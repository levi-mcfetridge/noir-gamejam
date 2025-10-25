using UnityEngine;

public class GuardScript : MonoBehaviour
{
    private float speed = 3f;
    private int direction = 0;
    private Vector3 startPos;
    private float moveDistance = 13f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private Transform player; // drag your player here

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // --- simple detection ---
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            PrisonScene.instance.caught = true;
        }

        // --- movement ---
        if (direction == 0) MoveUp();
        if (direction == 1) MoveRight();
        if (direction == 2) MoveDown();
        if (direction == 3) MoveLeft();

        if (Vector3.Distance(startPos, transform.position) >= moveDistance)
        {
            direction = (direction + 1) % 4;
            startPos = transform.position;
        }
    }

    private void MoveUp() => transform.position += new Vector3(0f, 0f, 1f) * speed * Time.deltaTime;
    private void MoveLeft() => transform.position += new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime;
    private void MoveRight() => transform.position += new Vector3(1f, 0f, 0f) * speed * Time.deltaTime;
    private void MoveDown() => transform.position += new Vector3(0f, 0f, -1f) * speed * Time.deltaTime;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
