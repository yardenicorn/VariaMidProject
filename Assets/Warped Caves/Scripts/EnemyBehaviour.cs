using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject enemyDeathParticlesPrefab;
    public GameObject enemyPrefab;
    private GameObject _enemyDeathParticlesPrefabInstance;

    private void Start()
    {
        _enemyDeathParticlesPrefabInstance = Instantiate(enemyDeathParticlesPrefab, transform.position, Quaternion.identity);
        _enemyDeathParticlesPrefabInstance.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
            _enemyDeathParticlesPrefabInstance.SetActive(true);
        }
    }
}
