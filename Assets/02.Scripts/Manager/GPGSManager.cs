using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using GooglePlayGames.OurUtils;

public class GPGSManager : MonoBehaviour
{
    [SerializeField] private Button goolgeLoginBtn;
    [SerializeField] private Button startBtn;
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
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif

    private InterstitialAd _interestitialAd;
    private const string fileName = "Fod_Player_Data";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        startBtn.interactable = false;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("IsLogined", 0) == 1)
        {
            Invoke("InitGPGS", 1f);
            goolgeLoginBtn.gameObject.SetActive(false);
        }
    }

    private void InitGPGS()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void SignIn()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // 로그인이 성공했을 때

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            GameManager.Instance.Nickname = name;
            //string id = PlayGamesPlatform.Instance.GetUserId();

            goolgeLoginBtn.gameObject.SetActive(false);
            startBtn.interactable = true;

            Debug.Log("Succes \n " + name);
            PlayerPrefs.SetInt("IsLogined", 1);

            MobileAds.Initialize(initStatus => { });
        }
        else
        {
            Debug.Log("Login Failed");
            goolgeLoginBtn.gameObject.SetActive(true);
        }
    }

    public void SaveGameData(string jsonData, Action<bool> callback)
    {

        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            callback?.Invoke(false);
            return;
        }

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(
            fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);
                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
                    savedGameClient.CommitUpdate(game, update, data, (saveStatus, updateGame) =>
                    {
                        if (saveStatus == SavedGameRequestStatus.Success)
                        {
                            Debug.Log("(구글)데이터 저장 성공");
                            callback?.Invoke(true);
                        }
                        else
                        {
                            Debug.Log("(구글)데이터 저장 실패");
                            callback?.Invoke(false);
                            Debug.Log(saveStatus);
                        }
                    });
                }
                else
                {
                    Debug.Log("파일 저장 실패(구글)");
                    Debug.Log(status);
                    callback?.Invoke(false);
                }
            }
        );
    }

    public void LoadGameData(Action<string> onLoaded)
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            onLoaded?.Invoke(null);
            return;
        }

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(
            fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    savedGameClient.ReadBinaryData(game, (readStatus, data) =>
                    {
                        if (readStatus == SavedGameRequestStatus.Success)
                        {
                            string json = Encoding.UTF8.GetString(data);
                            Debug.Log("(구글) 불러오기 성공");

                            onLoaded?.Invoke(json);
                        }
                        else
                        {
                            Debug.Log("(구글) 데이터 읽기 실패");
                            Debug.Log(status);

                            onLoaded?.Invoke(null);
                        }
                    });
                }
                else
                {
                    Debug.Log("(구글) 파일 열기 실패");
                    Debug.Log(status);

                    onLoaded?.Invoke(null);
                }
            }
        );
    }

    public void LoadInterstitialAd()
    {
        if (_interestitialAd != null)
        {
            _interestitialAd.Destroy();
            _interestitialAd = null;
        }

        Debug.Log("광고 로딩");

        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId, adRequest,
        (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("광고 로드 실패: " + error);

                return;
            }

            Debug.Log("광고 로드 성공: " + ad.GetResponseInfo());

            _interestitialAd = ad;
            RegisterEventHandlers(_interestitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if (_interestitialAd != null && _interestitialAd.CanShowAd())
        {
            Debug.Log("광고 출력");
            _interestitialAd.Show();
        }

        else
        {
            Debug.LogError("광고가 준비되지않았습니다.");
            LoadInterstitialAd();
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");

            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            LoadInterstitialAd();
        };
    }
}