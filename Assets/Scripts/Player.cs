using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] private GameInput gameInput;
     [SerializeField] private float moveSpeed = 7f;


     public Transform cameraTransform;

     private bool isWalking;

     private void Start()
     {
          Cursor.lockState = CursorLockMode.Locked;
     }

     private void Update()
     {
          HandleMovement();
     }


     private void HandleMovement()
     {
          Vector2 inputVector = gameInput.GetMovementVectorNormalized();
          Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

          // Make movement relative to camera direction
          Vector3 camForward = cameraTransform.forward;
          Vector3 camRight = cameraTransform.right;
          camForward.y = 0f;
          camRight.y = 0f;
          camForward.Normalize();
          camRight.Normalize();

          moveDir = (camForward * moveDir.z + camRight * moveDir.x).normalized;

          float moveDistance = moveSpeed * Time.deltaTime;
          float playerRadius = .7f;
          float playerHeight = 2f;
          bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

          if (!canMove)
          {
               Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
               canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

               if (canMove)
               {
                    moveDir = moveDirX;
               }
               else
               {
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                    if (canMove)
                    {
                         moveDir = moveDirZ;
                    }
               }
          }

          if (canMove)
          {
               transform.position += moveDir * moveDistance;
          }

          isWalking = moveDir != Vector3.zero;
          float rotateSpeed = 10f;
          if (moveDir != Vector3.zero)
               transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
     }

}
