using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleCloudSaveLoad : MonoBehaviour
{
    [SerializeField] Button saveBtn;
    [SerializeField] Button loadBtn;

    void Awake()
    {
        saveBtn.onClick.AddListener(SaveDataGPGS);
        loadBtn.onClick.AddListener(LoadDataGPGS);
    }

    private void SaveDataGPGS()
    {
        PlayerData data = SaveSystem.Instance.LoadData();

        SaveSystem.Instance.SaveDataWithGPGS(data);

        
    }

    private void LoadDataGPGS()
    {
        SaveSystem.Instance.LoadDataWithGPGS();
    }
}
