using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UnicornCounter : MonoBehaviour
{
    private int counter = 0;
    public GameObject youwinTextObject;
    public GameObject sparklesParticlesPrefab;
    private GameObject _sparklesParticlesInstance;

    private void Start()
    {
        _sparklesParticlesInstance = Instantiate(sparklesParticlesPrefab, transform.position, Quaternion.identity);
        _sparklesParticlesInstance.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unicorn"))
        {
            counter++;
            if (counter == 5)
            {
                youwinTextObject.SetActive(true);
                _sparklesParticlesInstance.SetActive(true);
            }
        }
    }
}
