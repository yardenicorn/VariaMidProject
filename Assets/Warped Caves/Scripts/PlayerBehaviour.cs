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
    private Transform _unicornFollowPoint;
    public GameObject bulletPrefab;
    public GameObject gate;
    public Collider2D gateColl;
    private HealthSystem _health;

    private bool _facingRight = true;
    private bool _isGrounded = true;
    private bool _ignoreInput = false;
    private bool _isShooting = false;
    [SerializeField] private float shootAnimDuration = 1f;
    private float currentShootTimer = 0f;
    private float _dirX;
    public float _hurtForce = 50f;


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private float feetRadius = 0.25f;
    [SerializeField] private int maxJumps = 2;
    private int jumpCounter = 0;
    [SerializeField] private PhysicsMaterial2D noFrictionMat;
    [SerializeField] private PhysicsMaterial2D frictionMat;

    private enum AnimationState { idle, run, jump, shoot, runshoot }
    private AnimationState state;
    
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
        _rb.velocity = new Vector2(_dirX * moveSpeed, _rb.velocity.y);
        if (_rb.velocity.x != 0)
        {
            _rb.sharedMaterial = noFrictionMat;
        }
        else
        {
            _rb.sharedMaterial = frictionMat;
        }
            

        if (Input.GetKeyDown("up") && jumpCounter < maxJumps)
        {
            _rb.velocity = Vector2.up * jumpForce;
            jumpCounter++;
        }

        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }

        currentShootTimer += Time.deltaTime;
        if (currentShootTimer > shootAnimDuration)
        {
            _isShooting = false;
        }

        GroundCheck();
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        _anim.SetFloat("speed", Mathf.Abs(_rb.velocity.x));
        _anim.SetBool("isGrounded", _isGrounded);

        if (_dirX > 0f)
        {
            state = AnimationState.run;
            _sprite.flipX = false;
            if (!_facingRight)
            {
                FlipFireandFollowPoint();
            }
        }
        else if (_dirX < 0f)
        {
            state = AnimationState.run;
            _sprite.flipX = true;
            if (_facingRight)
            {
                FlipFireandFollowPoint();
            }
        }
        else
        {
            state = AnimationState.idle;
        }

        if (_isGrounded)
        {
            state = AnimationState.jump;
        }


        if (Input.GetKeyDown("space"))
        {
            _isShooting = true;
            currentShootTimer = 0f;
            if (_dirX != 0f)
            {
                state = AnimationState.runshoot;
            }
            else
            {
                state = AnimationState.shoot;
            }
        }
        _anim.SetBool("isShooting", _isShooting);
        _anim.SetInteger("state", (int)state);
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
            //_health.TakeDamage();
            _anim.SetTrigger("Hurt");
            StartCoroutine(InputDisable());
            Vector2 direction = (Vector2)(transform.position - collision.transform.position).normalized;
            _rb.AddForce(direction * _hurtForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator InputDisable()
    {
        _ignoreInput = true;
        yield return new WaitForSeconds(0.5f);
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

    private void GroundCheck()
    {
        bool lastGroundState = _isGrounded;
        _isGrounded = Physics2D.OverlapCircle(playerFeet.position, feetRadius, groundLayer);
        if (lastGroundState == false && _isGrounded == true)
        {
            jumpCounter = 0;
        }
    }
}