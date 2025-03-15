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

    private int level;
    private float exp;
    private float nextExp;
    private int gold;
    private int dia;

    public int Level
    {
        set
        {
            level = Math.Max(0, value);

            if(mainUI != null)
                mainUI.SetLevel();
        }

        get => level;
    }
    public float Exp
    {
        set
        {
            exp = Math.Max(0, value);

            if(mainUI != null)
                mainUI.SetExpBar();

            if(exp >= nextExp)
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

            if(mainUI != null)
                mainUI.SetExpBar();
        }
        
        get => nextExp;
    }
    public int Gold
    {
        set 
        {
            gold = Math.Max(0, value);
            if(mainUI != null)
                mainUI.SetGold();
        }
        get => gold;
    }
    public int Dia
    {
        set
        {
            dia = Math.Max(0, value);

            if(mainUI != null)
                mainUI.SetDia();
        }
        get => dia;
    }
    [HideInInspector] public MainUIController mainUI;
    public string playerPrefabName;
    public Player player;
    GameObject GamePlayer;
    [HideInInspector] public bool isDragItem = false;



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

        Gold += 50000;
        Dia += 50000;
        Level = 5;
        exp = 500f;
        nextExp = 1000f;
    }

    virtual protected void LevelUp()
    {
        exp -= nextExp;
        NextExp *= 1.5f;

        Level += 1;

        mainUI.SetExpBar();
          
        if (exp >= nextExp)
        {
            LevelUp();
        }
    }

    public void BackToScene()
    {
        NetworkManager.Singleton.Shutdown();

        SceneManager.LoadScene("MainScene");
    }

    public void StartTutorial()
    {
        Task<string> code = ConnectRelay.Instance.CreateRelay();
        Debug.Log(code);
        StartCoroutine("LoadTutorial");
    }

    IEnumerator LoadTutorial()
    {
        LoadingScreen.Instance.ShowLoadingScreen();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TutorialScene");

        while(!asyncOperation.isDone){
            yield return null;
        }

        //yield return new WaitForSeconds(4f);
  
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.GetComponent<PlayerMovement>().enabled = true;
        
        RelicManager.Instance.ApplyRelics();
        LoadingScreen.Instance.HideLoadingScreen();
    }

    public void ChangeCharacter(string name)
    {
        playerPrefabName = name;
    }

    public void GameOver(ulong clientId)
    {
        // 게임 종료시 발생되는 함수
        Debug.Log("Game Over!");

        
    }
}
