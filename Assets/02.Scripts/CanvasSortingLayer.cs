using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSortingLayer : MonoBehaviour
{
    public Canvas _canvas;
    private SpriteRenderer _sr;
    public string _currentLayer;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _currentLayer = _sr.sortingLayerName;
    }


    // Update is called once per frame
    void Update()
    {
        // �÷��̾��� ���̾ �ٲ�� ĵ������ ���̾� �����
        if (_sr.sortingLayerName != _currentLayer)
        {
            _currentLayer = _sr.sortingLayerName;
            _canvas.sortingLayerName = _currentLayer;
        }
    }
}
