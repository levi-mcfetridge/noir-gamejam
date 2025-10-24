using System.IO.Pipes;
using UnityEditor.Build;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
     [SerializeField] private GameInput gameInput;
     [SerializeField] private float moveSpeed = 7f;
     [SerializeField] private float gravity = -9.81f;
     [SerializeField] private float jumpHeight = 2f;

     public Transform cameraTransform;

     private CharacterController controller;
     private Vector3 velocity;
     private bool isGrounded;

     private void Awake()
     {
          controller = GetComponent<CharacterController>();
     }

     private void Start()
     {
          Cursor.lockState = CursorLockMode.Locked;
     }

     private void Update()
     {
          HandleMovement();
          HandleInteractions();
     }

     private void HandleMovement()
     {
          // --- Ground Check ---
          isGrounded = controller.isGrounded;
          if (isGrounded && velocity.y < 0)
               velocity.y = -2f; // small push to stay grounded

          // --- Input ---
          Vector2 inputVector = gameInput.GetMovementVectorNormalized();
          Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

          // --- Camera-relative movement ---
          Vector3 camForward = cameraTransform.forward;
          Vector3 camRight = cameraTransform.right;
          camForward.y = 0f;
          camRight.y = 0f;
          camForward.Normalize();
          camRight.Normalize();

          moveDir = (camForward * moveDir.z + camRight * moveDir.x).normalized;

          // --- Apply movement ---
          if (gameInput.Sprint())
               controller.Move(moveDir * moveSpeed * Time.deltaTime * 1.5f); //Sprint
          else{
               controller.Move(moveDir * moveSpeed * Time.deltaTime); //Walk
          }

          // --- Jump ---
          if (isGrounded && gameInput.IsJumpPressed())
               velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

          // --- Apply gravity ---
          velocity.y += gravity * Time.deltaTime;
          controller.Move(velocity * Time.deltaTime);

          // --- Rotate toward move direction ---
          if (moveDir.magnitude > 0)
          {
               transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);
          }
          
     }
     private void HandleInteractions(){
          if (gameInput.Interact()){
               float interactRange = 2f;
               Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
               foreach (Collider collider in colliderArray){
                    //if(collider.TryGetComponent(out NPC npc)){
                    //     Debug.Log("cheese");
                    //}
               }
          }
     }
}
