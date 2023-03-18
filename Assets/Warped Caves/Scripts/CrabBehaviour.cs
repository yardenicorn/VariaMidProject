using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : MonoBehaviour
{
    public GameObject EnemyDeathPrefab;
    private GameObject enemyDeathInstance;
    public GameObject EnemyPrefab;
    private float respawnTime = 3f;

    private void Start()
    {
        enemyDeathInstance = Instantiate(EnemyDeathPrefab, transform.position, Quaternion.identity);
        enemyDeathInstance.SetActive(false);
    }

    private void OnDisable()
    {
        // Spawn Death Particles
        enemyDeathInstance.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void RespawnEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
    }

}
