using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    private bool isFollowing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }

    private void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
