using UnityEngine;

public class CityScene : MonoBehaviour
{
    public static CityScene instance;
    public GameObject robber;

    public bool talkedToRobber = false;
    public float robberSpeed = 5f;      // use it!
    public float moveDuration = 5f;     // expose in inspector if you like

    private bool startedMoving = false;
    private bool isLoadingScene = false; // NEW: prevents multiple loads
    private float moveTimer = 0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // If this object is meant to persist across scenes, uncomment:
        // DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Start movement exactly once after talking to the robber
        if (talkedToRobber && !startedMoving && !isLoadingScene)
        {
            startedMoving = true;
            moveTimer = 0f;
            // Optional: clear the trigger so it can't retrigger elsewhere
            talkedToRobber = false;
        }

        // If robber is moving, update position and timer
        if (startedMoving && !isLoadingScene)
        {
            if (robber != null)
            {
                // Move in a consistent direction using speed * deltaTime
                // (Your original code moved a fixed vector per frame)
                robber.transform.position -= new Vector3(-.4f, 0f, 3f);
            }

            moveTimer += Time.deltaTime;

            // After the move duration passes, load the next scene ONCE
            if (moveTimer >= moveDuration)
            {
                startedMoving = false;
                robberSpeed = 0f;

                if (!isLoadingScene)
                {
                    isLoadingScene = true;     // <— guard
                    enabled = false;           // optional extra safety
                    Debug.Log("Robber finished moving — loading next scene!");
                    if (SceneLoader.instance != null)
                        SceneLoader.instance.LoadNextScene();
                    else
                        Debug.LogWarning("SceneLoader.instance is null — add SceneLoader to your scene!");
                }
            }
        }
    }

    private void OnDisable()
    {
        // Extra safety to avoid phantom retriggers if this component gets disabled/enabled
        startedMoving = false;
    }
}
