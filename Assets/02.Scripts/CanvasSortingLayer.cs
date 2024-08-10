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
        // 플레이어의 레이어가 바뀌면 캔버스의 레이어 변경뮤
        if (_sg.sortingLayerName != _currentLayer)
        {
            _currentLayer = _sg.sortingLayerName;
            _canvas.sortingLayerName = _currentLayer;
        }
    }
}
