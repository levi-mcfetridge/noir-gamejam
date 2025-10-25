using Unity.VisualScripting;
using UnityEngine;

public class PrisonScene : MonoBehaviour
{
    // PrisonScene.cs
    public static PrisonScene instance;
    public GameObject lockObject;
    public GameObject finalDoor;
    public bool isCellLocked = true;


    private void Update()
    {
        if (!isCellLocked && lockObject) lockObject.SetActive(false);
    }


}
