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

    bool isAttacking = false;

    bool canCombo = false;
    float comboTimer = 0f;
    float comboWindow = 1f; // 1 segundo para pulsar F otra vez


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

        if (isAttacking)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                canCombo = true;
            }
            return;
        }

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

        // Frenar de vez
        if (Keyboard.current.sKey.wasPressedThisFrame)
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
            Attack(1);
        }

        animator.SetInteger("State", (int)state);

    }
    
    private void Attack(int attackNumber)
    {
        isAttacking = true;
        canCombo = false;
        comboTimer = 0f;

        if (attackNumber == 1)
            state = AnimationType.att1;
        else
            state = AnimationType.att2;

        animator.SetInteger("State", (int)state);
    }

    // Este método lo llamaremos desde el evento del final de la animación
    public void OnAttackEnd()
    {
        if (canCombo)
        {
            Attack(2);
        }
        else
        {
            isAttacking = false;
            state = AnimationType.idle;
            animator.SetInteger("State", (int)state);
        }
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
