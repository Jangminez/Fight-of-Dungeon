using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if(status == SignInStatus.Success)
        {
            // 로그인이 성공했을 때

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            Debug.Log("Succes \n " + name);
        }
        else{
            Debug.Log("Login Failed");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
