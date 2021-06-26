using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    private float _rotationSpeed = 100f;
    
    private Vector3 _currentAngle;
    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) OnRocketPressed();

        if(Input.GetKey(KeyCode.RightArrow)) RotateRight();
        
        if(Input.GetKey(KeyCode.LeftArrow)) RotateLeft();
        
    }


    private void RotateRight()
    {
        _currentAngle = transform.eulerAngles;
        _currentAngle -= new Vector3(0, 0, 1) * Time.deltaTime * _rotationSpeed;
        transform.eulerAngles = _currentAngle;
    }

    private void RotateLeft()
    {
        _currentAngle = transform.eulerAngles;
        _currentAngle += new Vector3(0, 0, 1) * Time.deltaTime * _rotationSpeed;
        transform.eulerAngles = _currentAngle;
    }

    private void OnRocketPressed()
    {
        _rb.AddRelativeForce(Vector2.down);
    }
}
