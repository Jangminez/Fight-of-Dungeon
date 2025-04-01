using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;

public class GPGSManager : MonoBehaviour
{
    private static GPGSManager instance;
    public static GPGSManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GPGSManager");
                instance = go.AddComponent<GPGSManager>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitGPGS();
        }
        else
            Destroy(gameObject);
    }

    private void InitGPGS()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // 로그인이 성공했을 때

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();

            Debug.Log("Succes \n " + name);
        }
        else
        {
            Debug.Log("Login Failed");
        }
    }

    public void SaveGameData(string jsonData)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(
            "player_data",
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    byte[] dtbyte = Encoding.UTF8.GetBytes(jsonData);
                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
                    savedGameClient.CommitUpdate(game, update, dtbyte, (saveStatus, updateGame) =>
                    {
                        if (saveStatus == SavedGameRequestStatus.Success)
                        {
                            Debug.Log("(구글)데이터 저장 성공");
                        }
                        else
                        {
                            Debug.Log("(구글)데이터 저장 실패");
                        }
                    });
                }
                else
                {
                    Debug.Log("파일 열기 실패(구글)");
                }
            }
        );
    }

    public string LoadGameData(string jsonData)
    {

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(
            "player_data",
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    savedGameClient.ReadBinaryData(game, (readStatus, data) =>
                    {
                        if (readStatus == SavedGameRequestStatus.Success)
                        {
                            jsonData = Encoding.UTF8.GetString(data);
                            Debug.Log("(구글) 불러온 데이터: " + jsonData);
                        }
                        else
                        {
                            Debug.Log("(구글) 데이터 읽기 실패");
                        }
                    });
                }
                else
                {
                    Debug.Log("(구글) 파일 열기 실패");
                }
            }
        );

        return jsonData;
    }


}
