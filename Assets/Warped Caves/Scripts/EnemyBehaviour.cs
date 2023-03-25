using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject enemyDeathPrefab;
    public GameObject enemyPrefab;
    private GameObject _enemyDeathInstance;
    // private float respawnTime = 3f;

    private void Start()
    {
        _enemyDeathInstance = Instantiate(enemyDeathPrefab, transform.position, Quaternion.identity);
        _enemyDeathInstance.SetActive(false);
    }

    private void OnDisable()
    {
        // Spawn Death Particles
        // _enemyDeathInstance.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    /*
    private void RespawnEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
    }
    */

}
