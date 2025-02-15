using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        LoadingScreen.Instance.ShowLoadingScreen();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while(!asyncLoad.isDone)
        {
            if(asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return new WaitUntil(() => NetworkManager.Singleton.IsConnectedClient 
                                    && NetworkManager.Singleton.LocalClient.PlayerObject.IsSpawned);

        LoadingScreen.Instance.HideLoadingScreen();
    }
}
