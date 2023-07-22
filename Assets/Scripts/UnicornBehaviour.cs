using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _followPoint;
    private SpriteRenderer _sprite;
    private float _movespeed = 7f;
    private bool _isFollowing = false;
    private bool _isinGarden = false;
    private float _delay = 2.0f;
    private float _timeElapsed = 0.0f;
    private float _dirX;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();  
    }

    private void Update()
    {
        if (_isFollowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, _followPoint.position, _movespeed * Time.deltaTime);
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= _delay)
            {
                _timeElapsed = 0.0f;
                previousPosition = _followPoint.position;
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
        if (!_isinGarden)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _isFollowing = true;
            }
        }

        if (collision.gameObject.CompareTag("GardenStay"))
        {
            _isFollowing = false;
            _isinGarden = true;
        }
    }
}
