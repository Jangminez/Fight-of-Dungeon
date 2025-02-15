using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : NetworkBehaviour
{
    public static SceneLoadManager Instance;
    public NetworkVariable<float> loadingProgress = new NetworkVariable<float>(0f);
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
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingProgress.Value = progress;
            
            LoadingScreen.Instance.ShowLoadingScreen();

            if (asyncOperation.progress >= 0.9f)
            {
                if(AreAllClientsLoaded())
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        LoadingScreen.Instance.HideLoadingScreen();
    }

    private bool AreAllClientsLoaded()
    {
        foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if(client.PlayerObject.TryGetComponent(out SceneLoadSync playerLoadStatus))
            {
                if(!playerLoadStatus.IsLoaded.Value)
                    return false;
            }
        }

        return true;
    }
}
