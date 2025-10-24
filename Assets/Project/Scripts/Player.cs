using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector3 moveDirNorm;
    private Animator anim;

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
          // --- Ground Check ---
          isGrounded = controller.isGrounded;
          if (isGrounded && velocity.y < 0f)
               velocity.y = -2f; // small push to stay grounded

          // --- Input ---
          Vector2 inputVector = gameInput.GetMovementVectorNormalized();
          Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

          // --- Camera-relative movement ---
          Vector3 camForward = cameraTransform.forward;
          camForward.y = 0f;
          camForward.Normalize();
          Vector3 camRight = cameraTransform.right;
          camRight.y = 0f;
          camRight.Normalize();

          moveDirNorm = (camForward * moveDir.z + camRight * moveDir.x).normalized;

          // --- Apply movement ---
          if (gameInput.Sprint())
               controller.Move(moveDirNorm * moveSpeed * Time.deltaTime * 1.5f); //Sprint
          else{
               controller.Move(moveDirNorm * moveSpeed * Time.deltaTime); //Walk
          }

          // --- Jump ---
          if (isGrounded && gameInput.IsJumpPressed())
          {
              velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
              anim?.SetTrigger("Jump");
          }
          // --- Apply gravity ---
          velocity.y += gravity * Time.deltaTime;
          controller.Move(velocity * Time.deltaTime);

          // --- Rotate toward move direction ---
          if (moveDirNorm.sqrMagnitude > 0.0001f)
          {
               transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);
          }
          
     }
    private void HandleAnimations()
    {
        if (!anim) return;

        // Use your computed moveDirNorm (0..1 magnitude) + sprint state
        float inputMag = new Vector2(moveDirNorm.x, moveDirNorm.z).magnitude;
        float speed01 = (gameInput.Sprint() ? 1f : 0.66f) * inputMag; // tune 0.66 for walk
        speed01 = Mathf.Clamp01(speed01);

        anim.SetFloat("Speed", speed01);

        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsSprinting", gameInput.Sprint());
    

    }
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
}
