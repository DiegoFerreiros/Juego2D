using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject[] vidas;
    public int vidaActual;
    [SerializeField] private TMP_Text cofresText;
    private Interactions interactions;

    private void Start()
    {
        vidaActual = vidas.Length;
        interactions = FindAnyObjectByType<Interactions>();
        ActualizarCofresUI();
        ActualizarVidasUI();
    }

    private void Update()
    {
        ActualizarCofresUI();
    }

    private void ActualizarCofresUI()
    {
        if (interactions != null)
        {
            cofresText.text = "x " + interactions.cofresRecogidos;
        }
    }

    public void RecibirDano()
    {
        if (vidaActual > 0)
        {
            vidaActual--;
            ActualizarVidasUI();
        }
    }

    private void ActualizarVidasUI()
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].SetActive(i < vidaActual);
        }
    }
}
