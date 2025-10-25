using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public KeyCode loadKey = KeyCode.L;   // key to go to next scene
    public bool loopToStart = false;      // optional: loop back to 0

    private static SceneLoader instance;  // singleton guard

    void Awake()
    {
        // make sure only one SceneLoader exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);    // stay alive through all scene loads
    }

    void Update()
    {
        if (Input.GetKeyDown(loadKey))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(next);
        }
        else if (loopToStart)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("No more scenes left — you're at the last one!");
        }
    }
}
