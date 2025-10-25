using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class NPC : MonoBehaviour
{
     [SerializeField] public string npcName = "";
     [SerializeField] public string Dialogue = "";


    public void Interact()
    {
        if (npcName == "John")
        {
                BarScene.instance.talkedToBartender = true;
                Debug.Log("talked to john");
        }
    }

}