using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
     [SerializeField] public string npcName = "";
     [SerializeField] public string Dialogue = "";


public void Interact()
     {
          Debug.Log(Dialogue);
     }
}