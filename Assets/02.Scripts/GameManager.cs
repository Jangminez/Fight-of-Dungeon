using System.Collections;
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

    public GameObject playerPrefab;
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
    }

    // 게임 시작
    public void StartGame()
    {
        player = playerPrefab.transform.GetChild(1).GetComponent<Player>();
        StartCoroutine(StartGameScene());

        GamePlayer = Instantiate(playerPrefab);
        player = GamePlayer.transform.GetChild(1).GetComponent<Player>();
        DontDestroyOnLoad(GamePlayer);
    }

    IEnumerator StartGameScene()
    {
        // 씬 비동기 로딩
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StageScene");

        // 씬이 로드될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // 씬이 로딩된 후 오브젝트 활성화 및 스폰
        player.gameObject.SetActive(true);
        player._spawnPoint = GameObject.FindWithTag("BlueSpawn").transform;
        player.transform.position = player._spawnPoint.position + new Vector3(0f, 1f, 0f);;

        UIManager.Instance.goToMain.onClick.AddListener(BackToScene);
    }

    public void BackToScene()
    {
        Destroy(GamePlayer);
        SceneManager.LoadScene("MainScene");
    }
}
