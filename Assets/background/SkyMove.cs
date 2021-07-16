using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMove : MonoBehaviour
{
    public float speed;
    
    private float _threshold = -150f;
    public event Action<GameObject> ONRemove; 
    
    private void Update()
    {
        transform.Translate(Vector2.left * (speed * Time.deltaTime));
        if(transform.position.x < _threshold) ONRemove?.Invoke(gameObject);
    }
}
