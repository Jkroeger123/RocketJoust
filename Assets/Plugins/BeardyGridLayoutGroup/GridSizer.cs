using System;
using System.Collections;
using System.Collections.Generic;
using Beardy;
using UnityEngine;

[ExecuteAlways]
public class GridSizer : MonoBehaviour
{
    public Canvas c;
    public float size = 4f;
    private GridLayoutGroup _layoutGroup;

    private float _cellSize;
    
    private void Start()
    {
        _layoutGroup = GetComponent<GridLayoutGroup>();
        _cellSize = size * c.scaleFactor;
        _layoutGroup.cellSize = new Vector2(_cellSize, _cellSize);
    }

    private void Update()
    {
        if (transform.childCount == 0) return;
        
        _cellSize = size * c.scaleFactor;
        _layoutGroup.cellSize = new Vector2(_cellSize, _cellSize);
    }
}
