using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

[Serializable]
public class RelicData
{
    public int r_Level;
    public int r_Count;
}

[Serializable]
public class PlayerData
{
    public string nickname;
    public int level;
    public float exp;
    public float nextExp;
    public int gold;
    public int dia;
    public Dictionary<int, RelicData> relicDict = new Dictionary<int, RelicData>();
}

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem _instance;
    public static SaveSystem Instance
    {
        get
        {
            // 싱글톤 구현
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SaveSystem)) as SaveSystem;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }
    private string filePath;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        else if (_instance != null)
            Destroy(gameObject);

        filePath = Application.persistentDataPath + "/PlayerData.json";

        DontDestroyOnLoad(gameObject);
    }

    public void SaveData(PlayerData data)
    {
        SavePlayerData(data);
        SaveRelicData(data);

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Debug.Log("데이터 JSON 저장 완료: " + filePath);
    }

    private void SavePlayerData(PlayerData data)
    {
        data.nickname = GameManager.Instance.Nickname;
        data.level = GameManager.Instance.Level;
        data.exp = GameManager.Instance.Exp;
        data.nextExp = GameManager.Instance.NextExp;
        data.gold = GameManager.Instance.Gold;
        data.dia = GameManager.Instance.Dia;
    }

    public void SaveRelicData(PlayerData data)
    {
        for (int i = 101; i <= 109; i++)
        {
            ScriptableRelic relic = RelicManager.Instance.GetRelic(i);

            data.relicDict[i].r_Level = relic.r_Level;
            data.relicDict[i].r_Count = relic.r_Count;
        }
    }

    public PlayerData LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
            Debug.Log("JSON 데이터 불러오기");

            return data;
        }

        else
        {
            Debug.LogWarning("저장된 JSON 데이터 없음, 새로 생성");
            return CreateNewPlayerData();
        }
    }

    private PlayerData CreateNewPlayerData()
    {
        PlayerData newData = new PlayerData();

        newData.nickname = "플레이어";
        newData.level = 1;
        newData.exp = 0;
        newData.nextExp = 100;
        newData.gold = 0;
        newData.dia = 0;
        newData.relicDict = new Dictionary<int, RelicData>();

        for (int i = 101; i <= 109; i++)
        {
            newData.relicDict[i] = new RelicData
            {
                r_Level = 1,
                r_Count = 0
            };
        }

        SaveData(newData);

        return newData;
    }
}
