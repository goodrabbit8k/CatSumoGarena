using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed = 100;

    void Start()
    {
        
    }

    void Update()
    {
        if (!FindObjectOfType<GameManager>().gameOver)
        {
            Move();
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, -speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}
