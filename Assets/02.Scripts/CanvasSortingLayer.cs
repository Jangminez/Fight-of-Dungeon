using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CanvasSortingLayer : MonoBehaviour
{
    public Canvas _canvas;
    private SortingGroup _sg;
    public string _currentLayer;

    private void Awake()
    {
        _sg = GetComponent<SortingGroup>();
        _currentLayer = _sg.sortingLayerName;
    }


    // Update is called once per frame
    void Update()
    {
        // �÷��̾��� ���̾ �ٲ�� ĵ������ ���̾� �����
        if (_sg.sortingLayerName != _currentLayer)
        {
            _currentLayer = _sg.sortingLayerName;
            _canvas.sortingLayerName = _currentLayer;
        }
    }
}
