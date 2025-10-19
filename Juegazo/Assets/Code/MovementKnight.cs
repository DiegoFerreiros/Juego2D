using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement1 : MonoBehaviour
{
    /*
    
    Checkpoints 
    Guia de controles al empezar a jugar 
    Menú de inicio y fin 

    */

    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D coll;
    [SerializeField] CapsuleCollider2D collNormal;
    [SerializeField] CapsuleCollider2D collCrouching;
    [SerializeField] CapsuleCollider2D collAttacking;

    [SerializeField] BoxCollider2D wallCheckLeft;
    [SerializeField] BoxCollider2D wallCheckRight;
    [SerializeField] LayerMask jumpableGround;
    [SerializeField] LayerMask leftWall;
    [SerializeField] LayerMask rightWall;
    [SerializeField] int vidas = 3;

    enum AnimationType { idle, running, crouching, jumping, transitioning, falling, attacking, death, wallSlide, crouchingTrans, crouchingWalk }
    AnimationType state = AnimationType.idle;

    SpriteRenderer sr;
    Animator animator;

    bool isCrouched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        collNormal.enabled = true;
        collCrouching.enabled = false;
    }

    void Update()
    {
        // --------------------- moverse en el suelo ----------------- 
        if (isGround())
        {
            if (Keyboard.current.ctrlKey.wasPressedThisFrame)
            {
                isCrouched = !isCrouched; // Toggle

                state = AnimationType.crouchingTrans;
                animator.SetInteger("state", (int)state);

                if (state == AnimationType.crouchingTrans)
                {
                    if (isCrouched)
                    {
                        // Inicia transición hacia agachado
                        collNormal.enabled = false;
                        collCrouching.enabled = true;
                        state = AnimationType.crouching;
                    }
                    else
                    {
                        // Inicia transición hacia levantado
                        collNormal.enabled = true;
                        collCrouching.enabled = false;
                        state = AnimationType.idle;
                    }
                }
            }

            // --- Si está agachado ---
            if (isCrouched)
            {
                if (Keyboard.current.dKey.isPressed)
                {
                    state = AnimationType.crouchingWalk;
                    sr.flipX = false;
                    if (rb.linearVelocityX <= 5)
                    {
                        rb.linearVelocityX += 0.1f;
                    }
                }
                else if (Keyboard.current.aKey.isPressed)
                {
                    state = AnimationType.crouchingWalk;
                    sr.flipX = true;
                    if (rb.linearVelocityX >= -5)
                    {
                        rb.linearVelocityX -= 0.1f;
                    }
                }
                else
                {
                    state = AnimationType.crouching;
                    rb.linearVelocityX = 0f;
                }
            }
            else
            {
                // --- Movimiento normal ---
                state = AnimationType.idle;

                if (Keyboard.current.dKey.isPressed)
                {
                    state = AnimationType.running;
                    sr.flipX = false;
                    if (rb.linearVelocityX <= 5)
                    {
                        rb.linearVelocityX += 0.2f;
                    }

                }
                else if (Keyboard.current.aKey.isPressed)
                {
                    state = AnimationType.running;
                    sr.flipX = true;
                    if (rb.linearVelocityX >= -5)
                    {
                        rb.linearVelocityX -= 0.2f;
                    }

                }
                else
                {
                    rb.linearVelocityX = 0f;
                }

                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    state = AnimationType.jumping;
                    rb.linearVelocityY = 5f;
                }
            }
        }

        // --------------------- moverse en el aire ----------------- 
        if (!isGround() && state != AnimationType.wallSlide)
        {
            if (Keyboard.current.dKey.isPressed)
            {
                sr.flipX = false;
                if (rb.linearVelocityX <= 5)
                {
                    rb.linearVelocityX += 0.1f;
                }
            }
            else if (Keyboard.current.aKey.isPressed)
            {
                sr.flipX = true;
                if (rb.linearVelocityX >= -5)
                {
                    rb.linearVelocityX -= 0.1f;
                }
            }
        }

        // --------------------- wall slide ----------------- 
        if (!isGround() && (isLeftWall() || isRightWall()))
        {
            state = AnimationType.wallSlide;

            float wallSlideSpeed = -2f;
            if (rb.linearVelocityY < wallSlideSpeed)
            {
                rb.linearVelocityY = wallSlideSpeed;
            }

            sr.flipX = isRightWall();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                state = AnimationType.jumping;
                float jumpX = isRightWall() ? -5f : 5f;
                float jumpY = 5f;
                rb.linearVelocity = new Vector2(jumpX, jumpY);
            }
        }
        else if (!isGround())
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                state = AnimationType.jumping;
            }
            else if (rb.linearVelocity.y <= 0.1f && rb.linearVelocity.y > -0.1f)
            {
                state = AnimationType.transitioning;
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                state = AnimationType.falling;
            }
        }

        if (isGround() && (state == AnimationType.falling || state == AnimationType.transitioning || state == AnimationType.jumping))
        {
            state = AnimationType.idle;
        }

        // --------------------- atacar ----------------- 
        if (Keyboard.current.fKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            state = AnimationType.attacking;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool isGround() => Physics2D.BoxCast(
        coll.bounds.center,
        coll.bounds.size,
        0f,
        Vector2.down,
        0.1f,
        jumpableGround
    );

    private bool isLeftWall() => Physics2D.BoxCast(
        wallCheckLeft.bounds.center,
        wallCheckLeft.bounds.size,
        0f,
        Vector2.left,
        0.1f,
        leftWall
    );

    private bool isRightWall() => Physics2D.BoxCast(
        wallCheckRight.bounds.center,
        wallCheckRight.bounds.size,
        0f,
        Vector2.right,
        0.1f,
        rightWall
    );

    private void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            state = AnimationType.death;
            ReiniciarJuego();
        }
    }
}
