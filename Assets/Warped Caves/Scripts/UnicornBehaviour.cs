using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornBehaviour : MonoBehaviour
{
    public Transform target;
    private float speed = 6f;
    private bool isFollowing = false;

    private void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFollowing = true;
        }

        if (collision.gameObject.CompareTag("GardenStay"))
        {
            isFollowing = false;
        }
    }
}
