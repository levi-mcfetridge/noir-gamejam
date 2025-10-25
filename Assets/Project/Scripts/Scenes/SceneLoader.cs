using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    public bool loopToStart = false;

    bool isLoading = false;   // <- guard

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextScene()
    {
        if (isLoading) return;          // ignore duplicates
        isLoading = true;

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;   // reset guard after load
            SceneManager.LoadScene(next);
        }
        else if (loopToStart)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("No more scenes left — you're at the last one!");
            isLoading = false;
        }
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        isLoading = false;
    }
}
