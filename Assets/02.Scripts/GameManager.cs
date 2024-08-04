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
            // �ν��Ͻ��� ���� ��쿡 �ν��Ͻ� �Ҵ�
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("���� �Ŵ����� ���������ʽ��ϴ�.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // �ν��Ͻ��� ���ٸ� ����
        if (_instance == null)
            _instance = this;

        // �ν��Ͻ��� �����ϸ� ���� ����� �ν��Ͻ� ����
        else if (_instance != null)
            Destroy(gameObject);

        // ���� ��ȯ �Ǿ ������ �ν��Ͻ��� �ı���������
        DontDestroyOnLoad(gameObject);
    }
}
