using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Bar");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
