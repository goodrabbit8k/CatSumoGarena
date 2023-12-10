using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject player;

    [SerializeField] public float followSpeed = 3f;
    [SerializeField] float distanceBetween = 1f;

    float distance;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward);
        }

        if (FindObjectOfType<GameManager>().gameOver)
        {
            followSpeed = 0;
        }
    }
}
