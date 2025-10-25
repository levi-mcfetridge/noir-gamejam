using UnityEngine;

public class Lock : MonoBehaviour
{

    public void Interact()
    {
        PrisonScene.instance.isCellLocked = false;
    }
}
