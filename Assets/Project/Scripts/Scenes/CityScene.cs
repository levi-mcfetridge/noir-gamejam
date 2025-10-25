using UnityEngine;

public class CityScene : MonoBehaviour
{
    public static CityScene instance;
    public GameObject robber;

    public bool talkedToRobber = false;
    public float robberSpeed = 0f;

    private bool startedMoving = false;
    private float moveTimer = 0f;
    private float moveDuration = 0f;  // how long the robber will move before scene change

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Update()
    {
        // When you talk to the robber, start his movement once
        if (talkedToRobber && !startedMoving)
        {
            robberSpeed = 5f; // units per second
            moveDuration = 5f;
            startedMoving = true;
            moveTimer = 0f;
            Debug.Log($"Robber starts moving for {moveDuration:F1} seconds");
        }

        // If robber is moving, update position and timer
        if (startedMoving)
        {
            if (robber != null)
            {
                robber.transform.position -= new Vector3(-.4f, 0f, 3f);
            }

            moveTimer += Time.deltaTime;

            // After the move duration passes, load the next scene
            if (moveTimer >= moveDuration)
            {
                startedMoving = false;
                robberSpeed = 0f;
                Debug.Log("Robber finished moving — loading next scene!");
                if (SceneLoader.instance != null)
                    SceneLoader.instance.LoadNextScene();
                else
                    Debug.LogWarning("SceneLoader.instance is null — add SceneLoader to your scene!");
            }
        }
    }
}
