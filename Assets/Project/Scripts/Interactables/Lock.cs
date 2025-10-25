using UnityEngine;

public class Lock : MonoBehaviour
{
    public void Interact()
    {
       
        // Show the padlock UI
        if (PadlockUI.Instance != null)
        {
            PadlockUI.Instance.Open(OnUnlocked); // pass a callback for when unlocked
        }
       
    }

    // Called by the Padlock UI when the player enters the correct code
    private void OnUnlocked()
    {
        PrisonScene.instance.isCellLocked = false;
        Debug.Log("Cell unlocked!");
    }
}
