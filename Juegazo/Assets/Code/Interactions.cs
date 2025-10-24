using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactions : MonoBehaviour
{
    public int cofresRecogidos = 0;
    enum AnimationType { idle, opening }
    float chestOpeningTime = 1f;
    private UIManager uiManager;

    [SerializeField] private float tiempoInvencible = 1f;
    private bool esInvencible = false;
    private float timerInvencible = 0f;

    public MovementKnight movementKnight;



    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();

        movementKnight = FindAnyObjectByType<MovementKnight>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chest"))
        {
            Chest chest = collision.GetComponent<Chest>();
            if (chest != null && !chest.recogido)
            {
                chest.recogido = true;
                cofresRecogidos++;

                Animator chestAnimator = collision.GetComponent<Animator>();
                if (chestAnimator != null)
                    chestAnimator.SetInteger("ChestState", (int)AnimationType.opening);

                Destroy(collision.gameObject, chestOpeningTime);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage"))
        {
            movementKnight.RecibirDano();
        }

        if (collision.CompareTag("castle"))
        {
            SceneManager.LoadScene("winMenu");
        }
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene("loseMenu");
    }
}
