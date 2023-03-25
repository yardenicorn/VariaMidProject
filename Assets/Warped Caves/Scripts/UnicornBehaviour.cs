using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornBehaviour : MonoBehaviour
{
    [SerializeField] private Transform FollowPoint;
    //private float speed = 6f;
    private bool isFollowing = false;
    private float delay = 1.0f;
    private float timeElapsed = 0.0f;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private void Update()
    {
        if (isFollowing)
        {
            OnTargetFollow();
            // transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
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

    private void OnTargetFollow()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= delay)
        {
            // Update the position and rotation of this GameObject
            transform.position = FollowPoint.position;
            transform.rotation = FollowPoint.rotation;

            // Reset the time elapsed and previous position/rotation
            timeElapsed = 0.0f;
            previousPosition = FollowPoint.position;
            previousRotation = FollowPoint.rotation;
        }
        else
        {
            // Interpolate the position and rotation between the previous and current player positions/rotations
            float t = timeElapsed / delay;
            transform.position = Vector3.Lerp(previousPosition, FollowPoint.position, t);
            transform.rotation = Quaternion.Slerp(previousRotation, FollowPoint.rotation, t);
        }
    }
}
