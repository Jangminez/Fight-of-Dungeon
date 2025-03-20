using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            // 싱글톤 구현
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }

    #region 플레이어 데이터
    private string nickname;
    private int level;
    private float exp;
    private float nextExp;
    private int gold;
    private int dia;

    public string Nickname
    {
        set
        {
            nickname = value;

            if(playerData != null)
                playerData.nickname = nickname;

            if (mainUI != null)
            {
                mainUI.SetNickName(nickname);
            }
        }

        get => nickname;
    }
    public int Level
    {
        set
        {
            level = Math.Max(0, value);

            if(playerData != null)
                playerData.level = level;

            if (mainUI != null)
                mainUI.SetLevel(level);
        }

        get => level;
    }
    public float Exp
    {
        set
        {
            exp = Math.Max(0, value);

            if(playerData != null)
                playerData.exp = exp;

            if (mainUI != null)
                mainUI.SetExpBar(playerData.exp, playerData.nextExp);

            if (playerData.exp >= playerData.nextExp)
            {
                LevelUp();
            }
        }

        get => exp;
    }
    public float NextExp
    {
        set
        {
            nextExp = Math.Max(0, value);

            if(playerData != null)
                playerData.nextExp = nextExp;

            if (mainUI != null)
                mainUI.SetExpBar(exp, nextExp);
        }

        get => nextExp;
    }
    public int Gold
    {
        set
        {
            gold = Math.Max(0, value);

            if(playerData != null)
                playerData.gold = gold;

            if (mainUI != null)
                mainUI.SetGold(gold);
        }
        get => gold;
    }
    public int Dia
    {
        set
        {
            dia = Math.Max(0, value);

            if(playerData != null)
                playerData.dia = dia;

            if (mainUI != null)
                mainUI.SetDia(dia);
        }
        get => dia;
    }
    #endregion
    [HideInInspector] public MainUIController mainUI;
    public string playerPrefabName;
    public Player player;
    public int rewardGold;
    public float rewardExp;

    public SaveSystem saveSystem;
    private PlayerData playerData;

    private void Awake()
    {
        // 인스턴스가 없을 때 해당 오브젝트로 설정
        if (_instance == null)
            _instance = this;

        // 인스턴스가 존재한다면 현재 오브젝트 파괴
        else if (_instance != null)
            Destroy(gameObject);

        // 씬 로드시에도 파괴되지않음 
        DontDestroyOnLoad(gameObject);
        saveSystem = SaveSystem.Instance;
    }

    void Start()
    {
        LoadPlayerData();
    }

    public void SavePlayerData()
    {
        if (playerData != null && saveSystem != null)
            saveSystem.SaveData(playerData);
    }

    private void LoadPlayerData()
    {
        try
        {
            playerData = saveSystem.LoadData();
            ApplyPlayerData();
            ApplyRelicData();
        }

        catch (Exception e)
        {
            Debug.LogError($"데이터 로드 실패: {e.Message}");
        }
    }

    private void ApplyPlayerData()
    {
        // JSON 데이터 적용
        Nickname = playerData.nickname;
        Gold = playerData.gold;
        Dia = playerData.dia;
        Level = playerData.level;
        NextExp = playerData.nextExp;
        Exp = playerData.exp;
    }

    private void ApplyRelicData()
    {
        for (int i = 101; i <= 109; i++)
        {
            ScriptableRelic relic = RelicManager.Instance.GetRelic(i);

            relic.r_Level = playerData.relicDict[i].r_Level;
            relic.r_Count = playerData.relicDict[i].r_Count;
            relic.r_UpgradeCost = 2000 + (1000 * (playerData.relicDict[i].r_Level - 1));
            relic.r_UpgradeCount = 5 + (3 * (playerData.relicDict[i].r_Level - 1));
            relic.r_UpgradeValue = (playerData.relicDict[i].r_Level - 1) * 2;
        }
    }

    private void LevelUp()
    {
        playerData.exp -= playerData.nextExp;
        NextExp *= 1.5f;

        Level += 1;

        mainUI.SetExpBar(playerData.exp, playerData.nextExp);

        if (playerData.exp >= playerData.nextExp)
        {
            LevelUp();
        }

        SavePlayerData();
        LoadPlayerData();
    }

    public void BackToScene()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);

        GameLobby.Instance.LeaveLobby();
        SceneManager.LoadScene("MainScene");
    }

    public void StartTutorial()
    {
        Task<string> code = ConnectRelay.Instance.CreateRelay();
        Debug.Log(code);
        StartCoroutine("LoadTutorial");
    }

    private IEnumerator LoadTutorial()
    {
        LoadingScreen.Instance.ShowLoadingScreen();

        while (!NetworkManager.Singleton.IsConnectedClient)
            yield return null;

        NetworkManager.Singleton.SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);

        NetworkManager.Singleton.SceneManager.OnLoadComplete += SetTutorialPlayer;
    }

    private void SetTutorialPlayer(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (sceneName == "TutorialScene")
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.GetComponent<PlayerMovement>().enabled = true;

            RelicManager.Instance.ApplyRelics();
            LoadingScreen.Instance.HideLoadingScreen();
        }

        NetworkManager.Singleton.SceneManager.OnLoadComplete -= SetTutorialPlayer;
    }

    public void ChangeCharacter(string name)
    {
        playerPrefabName = name;
    }

    public void GameOver()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);

        if (GameLobby.Instance.joinedLobby != null)
            GameLobby.Instance.LeaveLobby();

        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        LoadingScreen.Instance.ShowLoadingScreen();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainScene");

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        LoadingScreen.Instance.HideLoadingScreen();

        // 게임 종료시 골드와 경험치 지급
        Gold += rewardGold;
        Exp += rewardExp;

        // 플레이어 데이터 저장 & 불러오기
        SavePlayerData();
        LoadPlayerData();
    }

    public void GetPowerUp()
    {
        if (player != null)
        {
            player.Attack += 800f;
            player.Defense += 500f;
            player.AttackSpeed += 2f;
            player.MaxHp += 1000f;
            player.MaxMp += 1000f;
            player.HpRegen += 100f;
            player.MpRegen += 100f;
            player.Speed += 3f;
        }
    }
}
