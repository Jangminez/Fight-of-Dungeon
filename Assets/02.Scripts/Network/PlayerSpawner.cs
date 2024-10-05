using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;
    }

    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsHost && sceneName == "StageScene")
        {
            foreach (ulong id in clientsCompleted)
            {
                GameObject player = Instantiate(GameManager.Instance.playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

                GameManager.Instance.player = player.transform.GetChild(1).GetComponent<Player>();
                GameManager.Instance.SetPlayer();
            }
        }
    }
}
