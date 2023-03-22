using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Transform FirePoint;
    public GameObject bullet;
    private HealthSystem health;

    private bool facingRight = true;
    private bool isGrounded = true;
    private bool IgnoreInput = false;
    private float dirX = 0f;
    private float duckingOffset = 0.52f;
    public float hurtForce = 500f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum AnimationState { idle, run, jump, shot, duck, hurt, dead }
    private AnimationState state;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        health = GetComponent<HealthSystem>();
        FirePoint = transform.Find("Fire Point");
        health.onDeath.AddListener(onPlayerDeath);
    }
    void Shoot()
    {
        Instantiate(bullet, FirePoint.position, FirePoint.rotation);
    }

    private void Update()
    {
        if (IgnoreInput)
        {
            return;
        }

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
        if (Input.GetKeyDown("up"))
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {

        if (dirX > 0f)
        {
            state = AnimationState.run;
            sprite.flipX = false;
            if (!facingRight)
            {
                FlipFirePoint();
            }
        }
        else if (dirX < 0f)
        {
            state = AnimationState.run;
            sprite.flipX = true;
            if (facingRight)
            {
                FlipFirePoint();
            }
        }
        else
        {
            state = AnimationState.idle;
        }

        if (rb.velocity.y > 1)
        {
            state = AnimationState.jump;
        }

        if (Input.GetKeyDown("space"))
        {
            state = AnimationState.shot;
        }

        if (Input.GetKey("down"))
        {
            state = AnimationState.duck;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health.TakeDamage();
            anim.SetTrigger("Hurt");
            StartCoroutine(InputDisable());
            Vector2 direction = (Vector2)(transform.position - collision.transform.position).normalized + Vector2.up;
            rb.AddForce(direction * hurtForce, ForceMode2D.Impulse);
        }
        else if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    IEnumerator InputDisable()
    {
        IgnoreInput = true;
        yield return new WaitForSeconds(0.5f);
        IgnoreInput = false;
    }
    private void onPlayerDeath()
    {
        Debug.Log("onplayerdeathfunction");
        anim.SetTrigger("Dead");
    }
}