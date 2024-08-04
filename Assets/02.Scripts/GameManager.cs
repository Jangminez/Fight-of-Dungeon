using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 인스턴스 할당
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("게임 매니저가 존재하지않습니다.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 인스턴스가 없다면 생성
        if (_instance == null)
            _instance = this;

        // 인스턴스가 존재하면 새로 생기는 인스턴스 삭제
        else if (_instance != null)
            Destroy(gameObject);

        // 씬이 전환 되어도 선언한 인스턴스가 파괴되지않음
        DontDestroyOnLoad(gameObject);
    }
}
