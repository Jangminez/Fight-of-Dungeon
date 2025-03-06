using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : NetworkBehaviour
{
    public static SceneLoadManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneAsync(string sceneName)
    {
        if (IsServer)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        while(!CheckAllPlayersReady())
        {
            yield return null;
        }

    }

    public bool CheckAllPlayersReady()
    {
        foreach(var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (client.Value.PlayerObject == null)
                return false;

            if(client.Value.PlayerObject.TryGetComponent(out SceneLoadSync loadSync))
            {
                if(!loadSync.IsPlayerReady.Value)
                    return false;
            }
        }

        return true;
    }
}
