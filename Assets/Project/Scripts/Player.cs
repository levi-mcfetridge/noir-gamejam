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

    private CharacterController controller;
    private Animator anim;

    private Vector3 velocity;           // vertical velocity accumulator
    private bool isGrounded;
    private Vector3 moveDirNorm;        // camera-relative, planar

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        HandleAnimations();
        HandleInteractions();
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

        // animate from input magnitude (snappy & camera-relative)
        float inputMag = new Vector2(moveDirNorm.x, moveDirNorm.z).magnitude;    // 0..1
        float speed01 = (gameInput.Sprint() ? 1f : 0.66f) * inputMag;
        speed01 = Mathf.Clamp01(speed01);

        anim.SetFloat("Speed", speed01);                 // 1D blend tree param (Idle 0 / Walk .5 / Run 1)
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsSprinting", gameInput.Sprint());
    }
<<<<<<< HEAD

    private void HandleInteractions()
    {
        if (!gameInput.Interact()) return;

        const float interactRange = 2f;
        foreach (var col in Physics.OverlapSphere(transform.position, interactRange))
            if (col.TryGetComponent(out NPC npc))
                npc.Interact();
    }
=======
    private void HandleInteractions(){
          if (gameInput.Interact()){
               float interactRange = 2f;
               Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
               foreach (Collider collider in colliderArray){
                    Debug.Log(collider);
                    if (collider.TryGetComponent(out NPC npc))
                    {
                         npc.Interact();
                    }
               }
          }
     }
>>>>>>> 58f84cca15f3651f2c4dd7e4ce9bb3a8692a9cdf
}
