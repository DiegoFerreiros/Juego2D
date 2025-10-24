using UnityEngine;
using UnityEngine.SceneManagement;

public class loseSceneControl : MonoBehaviour
{
    [SerializeField] GameObject volverAlInicio;

    public void VolverInicio()
    {
        SceneManager.LoadScene("startMenu");
    }
}
