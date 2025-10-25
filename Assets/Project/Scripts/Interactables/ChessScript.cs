using UnityEngine;

public class ChessScript : MonoBehaviour
{

    public void Interact()
    {
        PrisonScene.instance.isFinalDoorLocked = false;
    }
}
