using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;

    public int enemyCount = 0;

    private SpawnManager spawnManager;
    private PlayerController playerController;

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (enemyCount == 10)
        {
            spawnManager.repeatRate = 2.5f;
            playerController.health = 4;
            playerController.attackRange = 0.6f;
            
        }
        else if (enemyCount == 20)
        {
            spawnManager.repeatRate = 2;
            playerController.health = 5;
            playerController.attackRange = 0.8f;
        }
        else if (enemyCount == 30)
        {
            spawnManager.repeatRate = 1f;
            playerController.health = 6;
            playerController.attackRange = 1f;
        }
    }
}
