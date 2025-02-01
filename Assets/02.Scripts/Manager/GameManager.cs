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
    private int gold;
    private int dia;
    public int Gold
    {
        set => gold = Math.Max(0, value);
        get => gold;
    }

    public int Dia
    {
        set => dia = Math.Max(0, value);
        get => dia;
    }

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

        Application.targetFrameRate = 60;
        // 씬 로드시에도 파괴되지않음 
        DontDestroyOnLoad(gameObject);

        Gold += 50000;
        Dia += 50000;
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
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TutorialScene");

        while(!asyncOperation.isDone){
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.GetComponent<PlayerMovement>().enabled = true;
    }
    public void ChangeCharacter(string name)
    {
        playerPrefabName = name;
    }

    public void GameOver(ulong clinetId)
    {
        // 게임 종료시 발생되는 함수
        Debug.Log("Game Over!");
    }
}
