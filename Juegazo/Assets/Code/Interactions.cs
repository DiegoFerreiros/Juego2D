using UnityEngine;

public class Interactions : MonoBehaviour
{
    int cofresRecogidos = 0;
    Animator animator;
    enum AnimationType { idle, opening }
    AnimationType state = AnimationType.idle;
    void Start()
    {
        
    }

    void Update()
    {

    }

    // Para cuando "atraviesas" el coleccionable se usa el OnTrigger
    // El onCollisionEnter2D es para cuando chocas con él
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            cofresRecogidos++;
            Destroy(collision.gameObject);
        }
    }
}
