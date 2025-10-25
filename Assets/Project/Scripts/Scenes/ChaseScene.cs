using UnityEngine;

public class ChaseScene : MonoBehaviour
{
    public GameObject truck;
    private GameObject[] cars;
    public GameObject carPrefab;
    void Start()
    {
        cars = GameObject.FindGameObjectsWithTag("Car");
        carPrefab.SetActive(true);
    }

    void Update()
    {
        truck.transform.position += new Vector3(-6f, 0f, .35f) * Time.deltaTime;
        foreach (GameObject car in cars)
        {
            if (car != null)
                car.transform.position += new Vector3(8f, 0f, -6/800f) * Time.deltaTime;
        }
    }
}
