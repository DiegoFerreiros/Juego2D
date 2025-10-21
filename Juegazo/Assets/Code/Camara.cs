using UnityEngine;

public class Camara : MonoBehaviour
{
    public Interactions interactions;
    public MovementKnight movementKnight;
    public Transform player;
    [SerializeField] GameObject muro1;
    [SerializeField] GameObject muro2;
    public float breakingTime = 1f;
    public float volverTime = 1f;

    private bool camaraDespegada = false;
    private bool esperando = false;
    private float timerVolver = 0f;
    private float timerEspera = 0f;
    public float retraso = 0.5f;
    private bool volviendoJugador = false;
    [SerializeField] private float velocidadCamara = 5f;


    void Start()
    {
        interactions = FindAnyObjectByType<Interactions>();
        movementKnight = FindAnyObjectByType<MovementKnight>();

        if (player == null && movementKnight != null)
        {
            player = movementKnight.transform;
        }
    }

    void Update()
    {

        if (interactions.cofresRecogidos == 3 && !camaraDespegada && !esperando && muro1 != null && muro2 != null)
        {
            esperando = true;
            timerEspera = 0f;
        }

        if (esperando)
        {
            timerEspera += Time.deltaTime;

            if (timerEspera >= retraso)
            {
                transform.parent = null;
                transform.position = new Vector3(
                    muro1.transform.position.x,
                    muro1.transform.position.y,
                    -10f
                );
                camaraDespegada = true;

                Destroy(muro1, breakingTime);
                Destroy(muro2, breakingTime);

                esperando = false;
            }
        }

        if (camaraDespegada)
        {
            movementKnight.rb.linearVelocityX = 0;
            movementKnight.rb.linearVelocityY = 0;

            timerVolver += Time.deltaTime;

            if (timerVolver >= breakingTime + volverTime)
            {
                volviendoJugador = true;
            }
        }

        if (volviendoJugador && player != null)
        {
            Vector3 objetivo = player.position + new Vector3(0f, 0f, -10f);

            transform.position = Vector3.Lerp(transform.position, objetivo, Time.deltaTime * velocidadCamara);

            if (Vector3.Distance(transform.position, objetivo) < 0.05f)
            {
                transform.position = objetivo;
                transform.parent = player;
                volviendoJugador = false;
                camaraDespegada = false;
            }
        }
    }
}
