using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform cameraTransform;      // set to your FreeLook rig's follow target (usually Player)

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float turnSpeed = 10f;          // how quickly the player aligns to camera heading

    [Header("Visual (optional)")]
    [SerializeField] private Transform modelTransform;        // drag YBot here (optional)
    [SerializeField] private float modelYawOffset = 0f;       // set to 90/-90/180 if your mesh faces sideways/back

    [SerializeField] private CinemachineBrain brain;

    public static Player instance;
    public bool canMove = true;

    private CharacterController controller;
    private Animator anim;

    private Vector3 velocity;           // vertical velocity accumulator
    private bool isGrounded;
    private Vector3 moveDirNorm;        // camera-relative, planar

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleAnimations();
            HandleInteractions();
        }
    }

    private void HandleMovement()
    {
        // --- Ground check ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f) velocity.y = -2f;

        // --- Input ---
        Vector2 input = gameInput.GetMovementVectorNormalized();
        Vector3 inputDir = new Vector3(input.x, 0f, input.y);   // local input vector (x,z)

        // --- Camera-relative basis (planar) ---
        Vector3 camF = cameraTransform.forward; camF.y = 0f; camF.Normalize();
        Vector3 camR = cameraTransform.right; camR.y = 0f; camR.Normalize();

        // world-space movement direction (planar)
        moveDirNorm = (camF * inputDir.z + camR * inputDir.x).normalized;

        // --- Move (walk/sprint) ---
        float speedMult = gameInput.Sprint() ? 1.5f : 1f;
        controller.Move(moveDirNorm * (moveSpeed * speedMult) * Time.deltaTime);

        // --- Jump ---
        if (isGrounded && gameInput.IsJumpPressed())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim?.SetTrigger("Jump");
        }

        // --- Gravity ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- Face the camera's yaw when there is input ---
        // This keeps camera independent, but the player aligns to camera heading while moving.
        if (moveDirNorm.sqrMagnitude > 0.0001f)
        {
            // target yaw taken from camera forward (planar)
            float camYaw = Mathf.Atan2(camF.x, camF.z) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.Euler(0f, camYaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSpeed);
        }

        // Keep a fixed local yaw offset on the visual model if your mesh forward != parent forward
        if (modelTransform) modelTransform.localRotation = Quaternion.Euler(0f, modelYawOffset, 0f);
    }

    private void HandleAnimations()
    {
        if (!anim) return;

        // Convert world move dir to local (player-relative) so X = strafe, Z = forward/back
        Vector3 localMove = transform.InverseTransformDirection(moveDirNorm);

        // If you use analog sticks, preserve partial input magnitude
        float inputMag = Mathf.Clamp01(new Vector2(localMove.x, localMove.z).magnitude);

        // Pick the tier that matches your blend tree coordinates
        float tier = gameInput.Sprint() ? 2f : 0.5f;      // run=2, walk=0.5

        // Final parameters match your tree's Pos X / Pos Y ranges ([-2..2])
        float vx = localMove.x * tier * inputMag;         // left/right
        float vz = localMove.z * tier * inputMag;         // fwd/back

        // Smooth damping to keep blends stable
        anim.SetFloat("Velocity X", vx, 0.1f, Time.deltaTime);
        anim.SetFloat("Velocity Z", vz, 0.1f, Time.deltaTime);

        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsSprinting", gameInput.Sprint());
    }
    private void HandleInteractions(){
          if (gameInput.Interact()){
               float interactRange = 2f;
               Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
               foreach (Collider collider in colliderArray){
                    //Debug.Log(collider); print all colliders
                    if (collider.TryGetComponent(out NPC npc))
                    {
                         npc.Interact();
                    }
                    if (collider.TryGetComponent(out Door door))
                    {
                        door.Interact();
                    }
                    if (collider.TryGetComponent(out Lock locks))
                    {
                        locks.Interact();
                    }
                    if (collider.TryGetComponent(out ChessScript chess))
                    {
                        chess.Interact();
                    }
            }
          }



     }
    public void SetPlayerActive(bool active)
    {
        // Deactivates or reactivates the entire Player GameObject
        gameObject.SetActive(active);
    }





}
