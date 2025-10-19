using UnityEngine;

public class Interactions : MonoBehaviour
{
    int cofresRecogidos = 0;
    enum AnimationType { idle, opening }
    float chestOpeningTime = 1f;

    // Para cuando "atraviesas" el coleccionable se usa el OnTrigger
    // El onCollisionEnter2D es para cuando chocas con él
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            Animator chestAnimator = collision.GetComponent<Animator>();

            if (chestAnimator != null)
            {
                chestAnimator.SetInteger("ChestState", (int)AnimationType.opening);
            }
            cofresRecogidos++;
            Destroy(collision.gameObject, chestOpeningTime);
        }
    }
}
