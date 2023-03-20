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

    //private void OnDisable()
    //{
    //    // Spawn Death Particles
    //    enemyDeathInstance.SetActive(true);
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
            Invoke("RespawnEnemy", respawnTime);
        }
    }

    private void RespawnEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
    }

}
