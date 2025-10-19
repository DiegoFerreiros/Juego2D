using UnityEngine;

public class FallingTile : MonoBehaviour
{
    enum AnimationType { idle, breaking }
    public float fallingTime = 0.01f;
    public float breakingTime = 0.02f;
    private Animator animator;
    private Rigidbody2D rb;
    private float timer = 0f;
    private bool playerTouched = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (playerTouched)
        {
            timer += Time.deltaTime;
            if (timer >= breakingTime)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 2f;
                playerTouched = false;
                Destroy(gameObject, fallingTime);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetInteger("fallingState", (int)AnimationType.breaking);
            }
            playerTouched = true;
            timer = 0f;
        }
    }
}
