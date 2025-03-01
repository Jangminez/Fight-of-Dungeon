using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : NetworkBehaviour
{
    public static SceneLoadManager Instance;
    public NetworkVariable<float> loadingProgress = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
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
            ShowLoadingClientRpc();
        }

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingProgress.Value = progress;

            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        while(!CheckAllPlayersReady())
        {
            yield return new WaitForSeconds(0.5f);
        }

        if(IsServer)
            HideLoadingClientRpc();
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

    [ClientRpc]
    public void ShowLoadingClientRpc()
    {
        LoadingScreen.Instance.ShowLoadingScreen();
    }

    [ClientRpc]
    public void HideLoadingClientRpc()
    {
        LoadingScreen.Instance.HideLoadingScreen();
    }
}
