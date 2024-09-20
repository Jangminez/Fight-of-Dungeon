using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [HideInInspector] public bool isDragItem = false;

    public Button startButton;

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

    void Start()
    {
        if(startButton != null)
            startButton.onClick.AddListener(StartGame);
    }


    void StartGame()
    {
        player = playerPrefab.transform.GetChild(1).GetComponent<Player>();
        StartCoroutine(StartGameScene());
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

        player._spawnPoint = GameObject.FindWithTag("BlueSpawn").transform;
        Instantiate(player.gameObject, player._spawnPoint.position, Quaternion.identity); 
    }
}
