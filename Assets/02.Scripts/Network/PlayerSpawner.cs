using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private CameraFollow _cam;
    private bool isSpawn = false;

    void Awake()
    {
        if(FindObjectOfType<PlayerSpawner>() != this)
        {
            Destroy(gameObject);
            return;
        }

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
            if (IsServer)
            {
                PlayerSpawnClientRpc(clientId);
            }
        }
    }

    [ClientRpc]
    public void PlayerSpawnClientRpc(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId && !isSpawn)
        {
            NetworkSpawnPlayerServerRpc(clientId, GameManager.Instance.playerPrefabName);
            Invoke("SpawnPlayer", 3f);
            isSpawn = true;
        }
    }

    private void SpawnPlayer()
    {
        // 각 매니저에 해당 클라이언트의 플레이어 설정
        GameManager.Instance.player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Player>();
        UIManager.Instance.player = GameManager.Instance.player;

        //플레이어 위치 설정
        if(IsServer)
        {
            GameManager.Instance.player._spawnPoint = GameObject.FindWithTag("BlueSpawn").transform;
        }
        else
        {
            GameManager.Instance.player._spawnPoint = GameObject.FindWithTag("RedSpawn").transform;
        }

        GameManager.Instance.player.transform.position = GameManager.Instance.player._spawnPoint.position + new Vector3(0f, 1f, 0f);
        SetUpCamera(GameManager.Instance.player.transform);
        GameManager.Instance.player.gameObject.SetActive(true);
        RelicManager.Instance.ApplyRelics();
    }

    private void SetUpCamera(Transform playerTransform)
    {
        // 플레이어 카메라 설정
        _cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
        _cam.target = playerTransform;
        _cam.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, _cam.transform.position.z);
        _cam.offset = _cam.transform.position - _cam.target.position;
    }

    [ServerRpc(RequireOwnership = false)]
    public void NetworkSpawnPlayerServerRpc(ulong clientId, string name)
    {
        NetworkObject playerObject = Instantiate(Resources.Load<GameObject>($"PlayerCharactors/{name}")).GetComponent<NetworkObject>();
        playerObject.SpawnAsPlayerObject(clientId, true);
    }
}
