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
    private HealthSystem _health;
    private Transform _firePoint;
    private Transform _unicornFollowPoint;
    public GameObject bulletPrefab;
    public GameObject gate;
    public Collider2D gateColl;

    private bool _facingRight = true;
    private bool _isGrounded = true;
    private bool _ignoreInput = false;
    private bool _isShooting = false;
    private bool _isDead = false;

    private float _dirX;
    private float _hurtForce = 20f;
    private float _moveSpeed = 7f;
    private float _jumpForce = 15f;
    private float _currentShootTimer = 0f;
    private float _shootAnimationDuration = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _playerFeet;
    private float _feetRadius = 0.25f;
    private int _jumpCounter = 0;
    private int _maxJumps = 2;
    [SerializeField] private PhysicsMaterial2D _noFrictionMat;
    [SerializeField] private PhysicsMaterial2D _frictionMat;

    private enum AnimationState { idle, run, jump, shoot, shootrun }
    private AnimationState _state;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _health = GetComponent<HealthSystem>();
        _firePoint = transform.Find("Fire Point");
        _unicornFollowPoint = transform.Find("Follow Point");
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
        _rb.velocity = new Vector2(_dirX * _moveSpeed, _rb.velocity.y);
        if (_rb.velocity.x != 0)
        {
            _rb.sharedMaterial = _noFrictionMat;
        }
        else
        {
            _rb.sharedMaterial = _frictionMat;
        }
            
        if (Input.GetKeyDown("up") && _jumpCounter < _maxJumps)
        {
            _rb.velocity = Vector2.up * _jumpForce;
            _jumpCounter++;
        }

        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }
        _currentShootTimer += Time.deltaTime;
        if (_currentShootTimer > _shootAnimationDuration)
        {
            _isShooting = false;
        }



        if (_isDead == true)
        {
            if (Input.anyKey)
            {
                _health.RestartGame();
            }
        }

        GroundCheck();
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        _anim.SetInteger("state", (int)_state);
        _anim.SetFloat("speed", Mathf.Abs(_rb.velocity.x));
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetBool("isShooting", _isShooting);

        if (_dirX > 0f)
        {
            _state = AnimationState.run;
            _sprite.flipX = false;
            if (!_facingRight)
            {
                FlipFireandFollowPoint();
            }
        }
        else if (_dirX < 0f)
        {
            _state = AnimationState.run;
            _sprite.flipX = true;
            if (_facingRight)
            {
                FlipFireandFollowPoint();
            }
        }
        else
        {
            _state = AnimationState.idle;
        }

        if (_isGrounded)
        {
            _state = AnimationState.jump;
        }

        if (Input.GetKeyDown("space"))
        {
            _isShooting = true;
            _currentShootTimer = 0f;
            if (_dirX != 0f)
            {
                _state = AnimationState.shootrun;
            }
            else
            {
                _state = AnimationState.shoot;
            }
        }
    }

    // function that makes the shot go right when the player faces right and go left when the player faces left
    private void FlipFireandFollowPoint()
    {
        _facingRight = !_facingRight;
        float rotation = _firePoint.rotation.eulerAngles.z;
        rotation = _facingRight ? 0f : 180f;
        _firePoint.rotation = Quaternion.Euler(0, 0, rotation);
        Vector3 tempPos = _firePoint.localPosition;
        Vector3 tempPos2 = _unicornFollowPoint.localPosition;
        tempPos.x = -tempPos.x;
        tempPos2.x = -tempPos2.x;
        _firePoint.localPosition = tempPos;
        _unicornFollowPoint.localPosition = tempPos2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _health.TakeDamage();
            _anim.SetTrigger("Hurt");
            StartCoroutine(InputDisable());
            Vector2 direction = (Vector2)(transform.position - collision.transform.position).normalized + Vector2.up;
            _rb.AddForce(direction * _hurtForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator InputDisable()
    {
        _ignoreInput = true;
        yield return new WaitForSeconds(0.7f);
        _ignoreInput = false;
    }
    private void onPlayerDeath()
    {
        _anim.SetTrigger("Dead");
        _isDead = true;
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

    private void GroundCheck()
    {
        bool lastGroundState = _isGrounded;
        _isGrounded = Physics2D.OverlapCircle(_playerFeet.position, _feetRadius, _groundLayer);
        if (lastGroundState == false && _isGrounded == true)
        {
            _jumpCounter = 0;
        }
    }
}