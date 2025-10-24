using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
     [SerializeField] public string npcName = "";
     [SerializeField] public List<string> Dialogue = new List<string>();


public void Interact()
     {
          foreach (string line in Dialogue)
          {
               Debug.Log(line);
          }
     }
}