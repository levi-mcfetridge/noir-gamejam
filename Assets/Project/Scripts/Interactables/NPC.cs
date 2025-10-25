using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] public string npcName = "NPC";
    [SerializeField] private Animator anim;                // John’s Animator (auto-found if left empty)
    [SerializeField] private string talkedBool = "talkedBool"; 


    [Header("Dialogue Lines")]
    [TextArea(2, 5)] public string[] lines;   // <-- put each line here in the Inspector


    private void Awake()
    {
        if (!anim) anim = GetComponentInChildren<Animator>();
    }
    public void Interact()
    {
        // your scene flags (optional)
        if (npcName == "John")
        {
            if (BarScene.instance) BarScene.instance.talkedToBartender = true;

            // ---> animate differently after talking
            if (anim)
            {
                anim.SetBool(talkedBool, true);     // switches John into the “after talk” state
                // anim.SetTrigger(greetTrigger);   // optional: play a one-shot greet
            }
        }
        if (npcName == "Robber")
        {
            if (CityScene.instance) CityScene.instance.talkedToRobber = true;
        }
        if (npcName == "HogFather")
        {
            if (PrisonScene.instance) PrisonScene.instance.talkedToHog = true;
            Debug.Log("Talked to HogFather");
        }

        // show dialogue
        if (DialogueUI.Instance)
        {
            DialogueUI.Instance.Show(lines);
        }
    }
}
