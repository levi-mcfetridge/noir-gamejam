using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
     public event EventHandler OnInteractAction;
     private PlayerInputActions playerInputActions;

     private void Awake()
     {
          playerInputActions = new PlayerInputActions();
          playerInputActions.Player.Enable();
     }

     public Vector2 GetMovementVectorNormalized()
     {
          Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
          return inputVector.normalized;
     }

     public bool IsJumpPressed()
     {
          return playerInputActions.Player.Jump.triggered;
     }

     public bool Sprint(){
          return playerInputActions.Player.Sprint.IsPressed();
     }

     public bool Interact(){
          return playerInputActions.Player.Interact.triggered;
     }

}
