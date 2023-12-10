using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] private int damage = 50;

    [SerializeField] private GameObject HitPointTop;
    [SerializeField] private GameObject HitPointBottom;

    [SerializeField] private GameObject hammerTop;
    [SerializeField] private GameObject hammerBottom;

    public int maxHealth = 3;
    public int health = 3;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public bool onTop;
    public bool onBottom;
    public bool canAttack = true;

    private Rigidbody2D playerRb;
    private PlayerInput playerInput;
    private Animator playerAnim;

    private InputAction moveAction;
    private InputAction attackAction;

    private GameManager gameManager;
    private Enemy enemy;

    [SerializeField] TextMeshProUGUI healthNumber;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();

        gameManager = FindObjectOfType<GameManager>();

        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];
    }

    private void Update()
    {
        healthNumber.text = health.ToString();

        Move();

        if (attackAction.triggered)
        {
            Attack();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(0, input.y, 0);

        transform.position += move * speed * Time.deltaTime;

        playerAnim.SetBool("isMove", true);

        if (move == new Vector3(0, 0))
        {
            playerAnim.SetBool("isMove", false);
        }

        if (input.y > 0)
        {
            HitPointTop.SetActive(true);
            HitPointBottom.SetActive(false);
            onTop = true;
            onBottom = false;
        }
        else if (input.y < 0)
        {
            HitPointTop.SetActive(false);
            HitPointBottom.SetActive(true);
            onTop = false;
            onBottom = true;
        }
    }

    private void Attack()
    {
        if (onTop && !onBottom && canAttack) 
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitPointTop.transform.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                hammerTop.SetActive(true);
                hammerBottom.SetActive(false);
                enemy.GetComponent<Enemy>().TakeDamage(damage);
                playerAnim.SetTrigger("isAttackUp");

                StartCoroutine(TurnOffHammerTop());
            }
        }
        else if (!onTop && onBottom && canAttack)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitPointBottom.transform.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                hammerTop.SetActive(false);
                hammerBottom.SetActive(true);
                enemy.GetComponent<Enemy>().TakeDamage(damage);
                playerAnim.SetTrigger("isAttackBottom");

                StartCoroutine(TurnOffHammerBottom());
            }
        }

        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator TurnOffHammerTop()
    {
        yield return new WaitForSeconds(0.5f);
        hammerTop.SetActive(false);
    }

    IEnumerator TurnOffHammerBottom()
    {
        yield return new WaitForSeconds(0.5f);
        hammerBottom.SetActive(false);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (HitPointTop == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(HitPointTop.transform.position, attackRange);

        if (HitPointBottom == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(HitPointBottom.transform.position, attackRange);
    }

    private void Die()
    {
        playerAnim.SetTrigger("isHit");
        gameManager.gameOver = true;
        speed = 0;
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Jurang")
        {
            Die();
        }        
    }
}
