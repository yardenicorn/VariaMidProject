using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private Transform _firePoint;
    public GameObject bulletPrefab;
    public GameObject gate;
    public Collider2D gateColl;
    private HealthSystem _health;

    private bool _facingRight = true;
    private bool _isGrounded = true;
    private bool _ignoreInput = false;
    private float _dirX;
    private float _hurtForce = 70f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum AnimationState { idle, run, jump, shoot, runshoot }
    private AnimationState state;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _health = GetComponent<HealthSystem>();
        _firePoint = transform.Find("Fire Point");
        _health.onDeath.AddListener(onPlayerDeath);
    }
    void Shoot()
    {
        Instantiate(bulletPrefab, _firePoint.position, _firePoint.rotation);
    }

    private void Update()
    {
        if (_ignoreInput)
        {
            return;
        }

        _dirX = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(_dirX * moveSpeed, _rb.velocity.y);

        if (Input.GetKeyDown("up"))
        {
            _rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {

        if (_dirX > 0f)
        {
            state = AnimationState.run;
            _sprite.flipX = false;
            if (!_facingRight)
            {
                FlipFirePoint();
            }
        }
        else if (_dirX < 0f)
        {
            state = AnimationState.run;
            _sprite.flipX = true;
            if (_facingRight)
            {
                FlipFirePoint();
            }
        }
        else
        {
            state = AnimationState.idle;
        }

        if (_rb.velocity.y > 1f)
        {
            state = AnimationState.jump;
        }

        if (Input.GetKeyDown("space"))
        {
            if (_dirX != 0f)
            {
                state = AnimationState.runshoot;
            }
            else
            {
                state = AnimationState.shoot;

            }
        }

        _anim.SetInteger("state", (int)state);
    }

    // function that makes the shot go right when the player faces right and go left when the player faces left
    private void FlipFirePoint()
    {
        _facingRight = !_facingRight;
        float rotation = _firePoint.rotation.eulerAngles.z;
        rotation = _facingRight ? 0f : 180f;
        _firePoint.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _health.TakeDamage();
            _anim.SetTrigger("Hurt");
            StartCoroutine(InputDisable());
            Vector2 direction = (Vector2)(transform.position - collision.transform.position).normalized;
            _rb.AddForce(direction * _hurtForce, ForceMode2D.Impulse);
        }
        else if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    IEnumerator InputDisable()
    {
        _ignoreInput = true;
        yield return new WaitForSeconds(1f);
        _ignoreInput = false;
    }
    private void onPlayerDeath()
    {
        _anim.SetTrigger("Dead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D gateColl = gate.GetComponent<Collider2D>();
        Animator GateAnimator = gate.GetComponent<Animator>();
        if (collision.gameObject.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            GateAnimator.SetTrigger("Open");
            gateColl.enabled = false;
        }
    }
}