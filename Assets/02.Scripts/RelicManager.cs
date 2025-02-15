using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    private static RelicManager _instance;
    public static RelicManager Instance
    {
        get
        {
            // 싱글톤 구현
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(RelicManager)) as RelicManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }
    private Dictionary<int, ScriptableRelic> relicDictionary = new Dictionary<int, ScriptableRelic>();

    void Awake()
    {
        // 인스턴스가 없을 때 해당 오브젝트로 설정
        if (_instance == null)
            _instance = this;

        // 인스턴스가 존재한다면 현재 오브젝트 파괴
        else if (_instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        LoadAllRelics();
    }

    private void LoadAllRelics()
    {
        relicDictionary.Clear();

        ScriptableRelic[] relics = Resources.LoadAll<ScriptableRelic>("Relics");
        foreach(var relic in relics)
        {
            if(relicDictionary.ContainsKey(relic.r_Id))
            {
                Debug.Log($"이미 존재하는 아이템입니다. {relic.r_Name}");
            }

            else
            {
                relicDictionary.Add(relic.r_Id, relic);
            }
        }
    }

    public ScriptableRelic GetRelic(int id)
    {
        if(relicDictionary.TryGetValue(id, out var relic))
        {
            return relic;
        }

        Debug.Log($"아이디에 해당하는 유물을 찾을 수 없습니다. \n 아이디: {id}");
        return null;
    }
}
