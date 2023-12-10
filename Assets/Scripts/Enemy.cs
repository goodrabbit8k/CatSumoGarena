using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform platform;

    public int maxHealth = 100;
    int currentHealth;

    private Rigidbody2D enemyRb;

    private EnemyAI enemyAI;
    private PlayerController playerController;
    private Animator enemyAnim;

    private GameManager gameManager;

    float currentFollowSpeed;

    public bool playerGotHit = false;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponentInChildren<EnemyAI>();
        enemyAnim = GetComponent<Animator>();

        gameManager = FindObjectOfType<GameManager>();

        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        currentHealth = 100;

        currentFollowSpeed = enemyAI.followSpeed;

        platform = GameObject.Find("Platform").transform;
    }

    private void Update()
    {
        transform.SetParent(platform.transform);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 50)
        {
            enemyAI.followSpeed = 0;
            enemyAnim.SetBool("gotKnocked", true);
            StartCoroutine(EnemyKnockdownCountdown());
        }

        if (currentHealth <= 0)
        {
            Die();
            enemyAnim.SetTrigger("gotFinish");
        }
    }

    IEnumerator EnemyKnockdownCountdown()
    {
        yield return new WaitForSeconds(2);
        enemyAI.followSpeed = currentFollowSpeed;
        currentHealth = maxHealth;
        enemyAnim.SetBool("gotKnocked", false);
    }

    private void Die()
    {
        if (playerController.onTop && !playerController.onBottom)
        {
            enemyRb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
        else if (!playerController.onTop && playerController.onBottom)
        {
            enemyRb.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
        }

        gameManager.enemyCount += 1;
        FindObjectOfType<UI>().score += 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject, 1f);
            enemyAnim.SetTrigger("isDie");
            playerController.health -= 1;
            playerGotHit = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Jurang")
        {
            Destroy(gameObject);
        }
    }
}