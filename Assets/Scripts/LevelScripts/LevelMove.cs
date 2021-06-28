using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMove : MonoBehaviour
{
    //public Rigidbody2D rbLevel;

    [SerializeField]
    public float gameSpeed = 4;

    Vector2 destroyPosition = new Vector2(-50, 0);

    void Start()
    {
        transform.Translate(Vector2.left * Time.deltaTime * gameSpeed);
    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * gameSpeed);

        if(transform.position.x <= destroyPosition.x)
        {
            checkOffScreen();
        }
    }

    void checkOffScreen()
    {
        Destroy(gameObject);
    }
}
