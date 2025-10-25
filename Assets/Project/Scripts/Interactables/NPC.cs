using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] public string npcName = "NPC";

    [Header("Dialogue Lines")]
    [TextArea(2, 5)] public string[] lines;   // <-- put each line here in the Inspector

    public void Interact()
    {
        // your scene flags (optional)
        if (npcName == "John")
        {
            if (BarScene.instance) BarScene.instance.talkedToBartender = true;
        }
        if (npcName == "Robber")
        {
            if (CityScene.instance) CityScene.instance.talkedToRobber = true;
        }

        // show dialogue
        if (DialogueUI.Instance)
        {
            DialogueUI.Instance.Show(lines);
        }
    }
}
