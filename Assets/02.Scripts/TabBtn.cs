using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabBtn : MonoBehaviour
{
    private Button _myBtn;
    public Transform _myTab;
    public Transform[] _otherTabs;

    private ColorBlock _cb;


    private void Awake()
    {
        _myBtn = GetComponent<Button>();

        _myBtn.onClick.AddListener(OpenTab);

        _cb = _myBtn.colors;
    }

    void OpenTab()
    {
        //_cb.normalColor = new Color32(152, 178, 221, 255);
        _myBtn.colors = _cb;
        _myTab.gameObject.SetActive(true);

        foreach(Transform other in _otherTabs)
        {
            other.gameObject.SetActive(false);
        }
    }
}
