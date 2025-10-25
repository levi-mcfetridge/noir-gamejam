using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string doorName = "ExitDoor";

    public void Interact()
    {
        if (BarScene.instance.talkedToBartender)
        {
            if (!BarScene.instance.isDoorUnlocked && doorName == "BarExitDoor")
            {
                BarScene.instance.isDoorUnlocked = true;
            }
        }
        else
        {
            Debug.Log("The door is locked. Maybe talk to the bartender first?");
        }
    }
}
