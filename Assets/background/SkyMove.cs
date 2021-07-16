using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMove : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate(Vector2.left * (speed * Time.deltaTime));
    }
}
