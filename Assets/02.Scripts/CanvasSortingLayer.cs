using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSortingLayer : MonoBehaviour
{
    public Canvas _canvas;
    public SpriteRenderer _player;
    public string _currentLayer;

    private void Awake()
    {
        _player = GetComponent<SpriteRenderer>();
        _currentLayer = _player.sortingLayerName;
    }


    // Update is called once per frame
    void Update()
    {
        // �÷��̾��� ���̾ �ٲ�� ĵ������ ���̾� �����
        if (_player.sortingLayerName != _currentLayer)
        {
            _currentLayer = _player.sortingLayerName;
            _canvas.sortingLayerName = _currentLayer;
        }
    }
}
