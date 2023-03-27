using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornBehaviour : MonoBehaviour
{
    [SerializeField] private Transform FollowPoint;
    private SpriteRenderer _sprite;
    private float speed = 7f;
    private bool isFollowing = false;
    private bool isinGarden = false;
    private float delay = 2.0f;
    private float timeElapsed = 0.0f;
    private float _dirX;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();  
    }

    private void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, FollowPoint.position, speed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= delay)
            {
                // Update the position and rotation of this GameObject
                transform.position = FollowPoint.position;
                //transform.rotation = FollowPoint.rotation;

                // Reset the time elapsed and previous position/rotation
                timeElapsed = 0.0f;
                previousPosition = FollowPoint.position;
                //previousRotation = FollowPoint.rotation;
            }
                
            _dirX = Input.GetAxisRaw("Horizontal");
            if (_dirX > 0f)
            {
                _sprite.flipX = false;
            }
            else if (_dirX < 0f)
            {
                _sprite.flipX = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isinGarden)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isFollowing = true;
            }
        }

        if (collision.gameObject.CompareTag("GardenStay"))
        {
            isFollowing = false;
            isinGarden = true;
        }
    }

    /*private void OnTargetFollow()
    {
        else
        {
            // Interpolate the position and rotation between the previous and current player positions/rotations
            float t = timeElapsed / delay;
            transform.position = Vector3.Lerp(previousPosition, FollowPoint.position, t);
            transform.rotation = Quaternion.Slerp(previousRotation, FollowPoint.rotation, t);
        }
    }*/
}
