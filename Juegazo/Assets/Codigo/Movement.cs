using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    //[SerializeField] EdgeCollider2D coll;
    [SerializeField] CapsuleCollider2D coll;
    [SerializeField] LayerMask jumpableGround;
    [SerializeField] int saltos = 0;

    [SerializeField] int vidas = 3;

    enum AnimationType { idle, running, jumping }
    AnimationType state = AnimationType.idle;
    SpriteRenderer sr;

    Animator animator;

    void Start()
    {
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
        //      4: att1
        //      5: att2
        //      6: death
        //      7: 


        if (Keyboard.current.dKey.isPressed())
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

        // Frenar de vez
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            rb.linearVelocityX = 0f;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround())
        {
            state = AnimationType.jumping;
            rb.linearVelocityY = 5;
            saltos = 1;
        }

        if (saltos == 1 && !isGround())
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                state = AnimationType.jumping;
                rb.linearVelocityY += 7;
                saltos = 2;
            }
        }

        if (saltos == 2)
        {
            saltos = 0;
        }

        animator.SetInteger("estado", (int)state);

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
        vidas--;
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
