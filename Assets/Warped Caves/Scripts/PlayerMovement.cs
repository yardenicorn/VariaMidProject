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
    private bool doublejump = true;
    private float dirX = 0f;
    public float duckingOffset = 0.52f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float JumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idle, run, jump, shot, duck, hurt }

    // Start is called before the first frame update
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
        // Debug.Log(doublejump);
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (IsGrounded() && !Input.GetKeyDown("up"))
        {
            doublejump = false;
        }

        if (Input.GetKeyDown("up"))
        {
            if (IsGrounded() || doublejump)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                doublejump = !doublejump;
            }
        }

        if (Input.GetKeyDown("up") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * 1f);
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

        if (Mathf.Abs(rb.velocity.y) > 1.5f)
        {
            state = MovementState.jump;
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

    // checking if the player is touching the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
