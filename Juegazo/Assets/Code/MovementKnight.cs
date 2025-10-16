using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement1 : MonoBehaviour
{

    /*

    Doble collider:
        Capsule para colisionar
        Box como sensor para detectar el suerlo y q flote en algunos sitios

    Variable de control para poder atacar y que no se cancele al instante con otra animacion

    Checkpoints

    Guia de controles al empezar a jugar

    Menú de inicio y fin



    */

    // state:
    // 0: idle 
    // 1: run 
    // 2: slide
    // 3: jump
    // 4: jump -> fall 
    // 5: fall -> debería activarse despues de la transicion de salto a caida 
    //            o cuando velocidad Y es negativa (así siempre q se caiga, sin necesidad de saltar, se activa)
    // 6: attack
    // 7: death
    // 8: wall slide -> intentar lo del wall jump
    // 9: turn around si me da tiempo

    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D coll;
    [SerializeField] CapsuleCollider2D collNormal;
    [SerializeField] CapsuleCollider2D collSliding;
    [SerializeField] BoxCollider2D wallCheckLeft;
    [SerializeField] BoxCollider2D wallCheckRight;
    [SerializeField] LayerMask jumpableGround;
    [SerializeField] LayerMask leftWall;
    [SerializeField] LayerMask rightWall;
    [SerializeField] int vidas = 3;
    enum AnimationType { idle, running, sliding, jumping, transitioning, falling, attacking, death, wallSlide, turnAround }
    AnimationType state = AnimationType.idle;
    SpriteRenderer sr;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        rb.gravityScale = 1f;
        state = AnimationType.idle;
        collNormal.enabled = true;
        collSliding.enabled = false;

        if (isGround())
        {
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
        }

        if (!isGround())
        {
            if (Keyboard.current.dKey.IsPressed())
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
            else
            {
                rb.linearVelocityX = 0f;
            }
        }
        

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround())
        {
            state = AnimationType.jumping;
            rb.linearVelocityY = 5;
        }

        if (isRightWall() && !isGround())
        {
            sr.flipX = true;
            state = AnimationType.wallSlide;
            rb.gravityScale = 0.2f;
        }
        else if (isLeftWall() && !isGround())
        {
            sr.flipX = false;
            state = AnimationType.wallSlide;
            rb.gravityScale = 0.2f;
        }
        else if (!isGround())
        {
            // Si todavia sube
            if (rb.linearVelocity.y > 0.1f)
            {
                state = AnimationType.jumping;
            }
            // Si está justo en el punto alto (ni sube ni cae)
            else if (rb.linearVelocity.y <= 0.1f && rb.linearVelocity.y > -0.1f)
            {
                state = AnimationType.transitioning;
            }
            // Si ya cae con fuerza
            else if (rb.linearVelocity.y < -0.1f)
            {
                state = AnimationType.falling;
            }
        }

        if (isGround() && (state == AnimationType.falling || state == AnimationType.transitioning || state == AnimationType.jumping))
        {
            state = AnimationType.idle;
        }

        if (Keyboard.current.fKey.isPressed)
        {
            state = AnimationType.attacking;
        }

        if (Keyboard.current.shiftKey.isPressed)
        {
            collNormal.enabled = false;
            collSliding.enabled = true;
            state = AnimationType.sliding;
        }

        animator.SetInteger("state", (int)state);

    }


    private bool isGround() => Physics2D.BoxCast(
        coll.bounds.center,
        coll.bounds.size,
        0f,
        Vector2.down,
        .1f,
        jumpableGround
    );

    private bool isLeftWall() => Physics2D.BoxCast(
        wallCheckLeft.bounds.center,
        wallCheckLeft.bounds.size,
        0f,
        Vector2.left,
        .1f,
        leftWall
    );

    private bool isRightWall() => Physics2D.BoxCast(
        wallCheckRight.bounds.center,
        wallCheckRight.bounds.size,
        0f,
        Vector2.right,
        .1f,
        rightWall
    );
    

    private void ReiniciarJuego()
    {
        //vidas--;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
