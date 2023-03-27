using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UnicornCounter : MonoBehaviour
{
    private int counter = 0;
    public GameObject youwinTextObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unicorn"))
        {
            counter++;
            if (counter == 5)
            {
                youwinTextObject.SetActive(true);
            }
        }
    }
}
