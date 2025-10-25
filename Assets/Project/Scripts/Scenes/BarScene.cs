using UnityEngine;

public class BarScene : MonoBehaviour
{
    public static BarScene instance;

    public bool talkedToBartender = false;
    public bool isDoorUnlocked = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    public void Start()
    {
        PauseMenu.instance.pauseMenu.SetActive(false);
        PauseMenu.instance.isPaused = false;
        Time.timeScale = 1f;

    }
    void Update()
    {
        if (talkedToBartender && isDoorUnlocked)
        {
            SceneLoader.instance.LoadNextScene();
            talkedToBartender = false;
        }
    }
}
