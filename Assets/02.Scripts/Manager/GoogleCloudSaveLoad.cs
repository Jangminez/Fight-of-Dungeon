using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GoogleCloudSaveLoad : MonoBehaviour
{
    [SerializeField] Button saveBtn;
    [SerializeField] Button loadBtn;
    [SerializeField] CanvasGroup successInfo;
    [SerializeField] CanvasGroup failedInfo;

    void Awake()
    {
        saveBtn.onClick.AddListener(SaveDataGPGS);
        loadBtn.onClick.AddListener(LoadDataGPGS);
    }

    private void SaveDataGPGS()
    {
        PlayerData data = SaveSystem.Instance.LoadData();

        if(SaveSystem.Instance.SaveDataWithGPGS(data))
        {
            successInfo.transform.GetChild(0).GetComponent<Text>().text = "데이터 저장 성공";
            
            successInfo.DOFade(1f, 1f)
            .OnComplete(() => 
            DOVirtual.DelayedCall(1f, () => successInfo.DOFade(0f, 1f).SetEase(Ease.InOutSine)));
        }

        else
        {
            failedInfo.transform.GetChild(0).GetComponent<Text>().text = "데이터 저장 실패";
            
            failedInfo.DOFade(1f, 1f)
            .OnComplete(() => 
            DOVirtual.DelayedCall(1f, () => failedInfo.DOFade(0f, 1f).SetEase(Ease.InOutSine)));
        }
    }

    private void LoadDataGPGS()
    {
        PlayerData data = SaveSystem.Instance.LoadDataWithGPGS();

        if(data != null)
        {
            successInfo.transform.GetChild(0).GetComponent<Text>().text = "데이터 불러오기 성공";
            
            successInfo.DOFade(1f, 1f)
            .OnComplete(() => 
            DOVirtual.DelayedCall(1f, () => successInfo.DOFade(0f, 1f).SetEase(Ease.InOutSine)));
        }

        else
        {
            failedInfo.transform.GetChild(0).GetComponent<Text>().text = "데이터 불러오기 실패";
            
            failedInfo.DOFade(1f, 1f)
            .OnComplete(() => 
            DOVirtual.DelayedCall(1f, () => failedInfo.DOFade(0f, 1f).SetEase(Ease.InOutSine)));
        }
    }
}
