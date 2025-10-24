using UnityEngine;

public class NPC : MonoBehaviour
{
     public string npcName = "Unnamed NPC";

     public void Interact()
     {
          Debug.Log($"{npcName} says: Hello traveler!");
          // You can trigger dialogue, quests, shop menus, etc. here later.
     }
}