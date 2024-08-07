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
        // 플레이어의 레이어가 바뀌면 캔버스의 레이어 변경뮤
        if (_sr.sortingLayerName != _currentLayer)
        {
            _currentLayer = _sr.sortingLayerName;
            _canvas.sortingLayerName = _currentLayer;
        }
    }
}
