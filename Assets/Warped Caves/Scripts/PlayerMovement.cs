using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Transform FirePoint;
    public GameObject bullet;

    private bool facingRight = true;
    private float dirX = 0f;
    private float duckingOffset = 0.52f;
    private int jumpingCount = 0;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float JumpForce = 16f;
    [SerializeField] private LayerMask Ground;

    private enum MovementState { idle, run, jump, shot, duck, fall }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        FirePoint = transform.Find("Fire Point");
    }
    void Shoot()
    {
        Instantiate(bullet, FirePoint.position, FirePoint.rotation);
    }

    // Update is called once per frame
    private void Update()
    {
        bool isGrounded = Physics2D.Linecast(transform.position,transform.position + Vector3.down, LayerMask.GetMask("Ground"));

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpingCount < 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            jumpingCount++;
        }

        if (isGrounded)
        {
            jumpingCount = 0;
        }

        UpdateAnimationState();

        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.run;
            sprite.flipX = false;
            if (!facingRight)
            {
                FlipFirePoint();
            }
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            sprite.flipX = true;
            if (facingRight)
            {
                FlipFirePoint();
            }
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 3.5f)
        {
            Debug.Log(rb.velocity.y);
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            Debug.Log(rb.velocity.y);
            state = MovementState.fall;
        }

        if (Input.GetKeyDown("space"))
        {
            state = MovementState.shot;
        }

        if (Input.GetKey("down"))
        {
            state = MovementState.duck;
            Vector3 newFirePointPosition = FirePoint.position;
            newFirePointPosition.y -= duckingOffset;
            FirePoint.position = newFirePointPosition;
        }

        anim.SetInteger("state", (int)state);
    }

    // function that makes the shot go right when the player faces right and go left when the player faces left
    private void FlipFirePoint()
    {
        facingRight = !facingRight;

        float rotation = FirePoint.rotation.eulerAngles.z;
        rotation = facingRight ? 0f : 180f;
        FirePoint.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
