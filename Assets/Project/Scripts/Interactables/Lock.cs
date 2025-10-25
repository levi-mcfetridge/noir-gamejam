using UnityEngine;

public class Lock : MonoBehaviour
{

    public void Interact()
    {
        if(PrisonScene.instance.talkedToHog)
            PrisonScene.instance.isCellLocked = false;
    }
}
