using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CapsuleCollider2D coll;
    [SerializeField] LayerMask jumpableGround;
    [SerializeField] int vidas = 3;

    enum AnimationType { idle, running, jumping, falling, att1, att2, death }
    AnimationType state = AnimationType.idle;
    SpriteRenderer sr;


    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        state = AnimationType.idle;

        // state:
        //      0: idle
        //      1: run
        //      2: jump
        //      3: fall
        //      4: attack
        //      5: death


        if (Keyboard.current.dKey.IsPressed())
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

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround())
        {
            state = AnimationType.jumping;
            rb.linearVelocityY = 5;
        }

        if (rb.linearVelocity.y < -0.1f && !isGround())
        {
            state = AnimationType.falling;
        }


        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            state = AnimationType.att1;
        }

        animator.SetInteger("State", (int)state);

    }


    private bool isGround() => Physics2D.BoxCast(
        coll.bounds.center,
        coll.bounds.size,
        0f,
        Vector2.down,
        .1f,
        jumpableGround
    );

    private void ReiniciarJuego()
    {
        //vidas--;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Muerte"))
        {
            ReiniciarJuego();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
