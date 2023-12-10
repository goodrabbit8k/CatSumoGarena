using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemy;

    public float repeatRate = 5f;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(repeatRate);

        int randomSpawnPoints = Random.Range(0, 3);
        Instantiate(enemy, spawnPoints[randomSpawnPoints].position, enemy.transform.rotation);

        StartCoroutine(SpawnEnemy());
    }
}
