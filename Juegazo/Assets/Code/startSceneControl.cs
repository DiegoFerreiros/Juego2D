using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesControl : MonoBehaviour
{
    [SerializeField] GameObject pressAnyKeyButton;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject exitButton;
    void Start()
    {
        pressAnyKeyButton.SetActive(true);
        startButton.SetActive(false);
        exitButton.SetActive(false);
    }

    public void PressAnyKey()
    {
        pressAnyKeyButton.SetActive(false);
        startButton.SetActive(true);
        exitButton.SetActive(true);    
    }

    public void Jugar()
    {
        SceneManager.LoadScene("main");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
