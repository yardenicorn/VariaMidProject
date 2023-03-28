using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject enemyDeathParticlesPrefab;
    public GameObject enemyPrefab;
    private GameObject _enemyDeathInstance;
    // private float respawnTime = 3f;

    private void Start()
    {
        _enemyDeathInstance = Instantiate(enemyDeathParticlesPrefab, transform.position, Quaternion.identity);
        _enemyDeathInstance.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // Spawn Death Particles
        _enemyDeathInstance.SetActive(true);
    }
}
