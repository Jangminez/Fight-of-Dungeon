using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private CameraFollow _cam;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public override void OnNetworkSpawn()
    {
        // 씬이 로드 되면 플레이어 오브젝트 생성
        NetworkManager.Singleton.SceneManager.OnLoadComplete += SpawnPlayerObject;
    }

    private void SpawnPlayerObject(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (sceneName == "StageScene")
        {
            if (IsHost)
            {
                GameObject player = Instantiate(GameManager.Instance.playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
                SetPlayerClientRpc(clientId);
            }
        }
    }

    [ClientRpc]
    public void SetPlayerClientRpc(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            // 각 매니저에 클라이언트의 플레이어 설정
            GameManager.Instance.player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().transform.GetChild(1).GetComponent<Player>();
            UIManager.Instance.player = GameManager.Instance.player;

            GameManager.Instance.player.gameObject.SetActive(true);
            GameManager.Instance.player._spawnPoint = GameObject.FindWithTag("BlueSpawn").transform;
            GameManager.Instance.player.transform.position = GameManager.Instance.player._spawnPoint.position + new Vector3(0f, 1f, 0f); ;

            UIManager.Instance.goToMain.onClick.AddListener(GameManager.Instance.BackToScene);


            // 플레이어 카메라 설정
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
            _cam.target = GameManager.Instance.player.transform;
            _cam.transform.position = new Vector3(_cam.target.position.x, _cam.target.position.y, _cam.transform.position.z);
            _cam.offset = _cam.transform.position - _cam.target.position;
        }
    }
}
