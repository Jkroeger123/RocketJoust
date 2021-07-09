using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PhantomImage : MonoBehaviour
{
    private const float DisappearSpeed = 0.2f;
    private SpriteRenderer _spriteRenderer;
    private Color _startColor, _finalColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _startColor = Color.white;
        _startColor.a = 0.5f;
        
        _finalColor = Color.white;
        _finalColor.a = 0f;
        
        _spriteRenderer.color = _startColor;
        
        _spriteRenderer.DOColor(_finalColor, DisappearSpeed).OnComplete(() => Destroy(gameObject));
    }
}