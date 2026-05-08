using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject activeCharacter;
    [SerializeField] Transform cameraTransform;
    
    [Header("Movement Settings")]
    [SerializeField] float speed = 8f;
    [SerializeField] float sprintSpeed = 15f;
    [SerializeField] float rotateSpeed = 4f;
    [SerializeField] float gravityValue = -10f;
    [SerializeField] float mouseSensitivity = 2f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float xRotation = 0f;

    // Mobile Input variables
    private float inputVertical;
    private float inputHorizontal;
    private bool isSprintingToggled;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void SetMoveVertical(float val) => inputVertical = val;
    public void SetMoveHorizontal(float val) => inputHorizontal = val;
    
    public void ToggleSprinting() 
    {
        isSprintingToggled = !isSprintingToggled;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Small downward force to stay grounded
        }

        // 1. COMBINE INPUTS
        float v = inputVertical + Input.GetAxisRaw("Vertical");
        float h = inputHorizontal + Input.GetAxisRaw("Horizontal");
        bool currentlySprinting = (isSprintingToggled || Input.GetKey(KeyCode.LeftShift)) && v > 0;

        // 2. CAMERA/ROTATION (Mouse — desktop only)
        if (!Application.isMobilePlatform)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // 3. MOVEMENT LOGIC
        // Rotate the player based on Horizontal input (A/D buttons)
        transform.Rotate(0, h * rotateSpeed, 0);
        
        // Calculate Directional Movement
        float curSpeed = (currentlySprinting ? sprintSpeed : speed) * v;
        Vector3 moveDirection = transform.forward * curSpeed;

        // Apply Horizontal Movement
        controller.Move(moveDirection * Time.deltaTime);

        // 4. GRAVITY LOGIC
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // 5. ANIMATION LOGIC
        if (Mathf.Abs(v) > 0.1f || Mathf.Abs(h) > 0.1f)
        {
            controller.minMoveDistance = 0.001f;
            if (currentlySprinting)
                activeCharacter.GetComponent<Animator>().Play("Standard Run");
            else
                activeCharacter.GetComponent<Animator>().Play("Walking");
        }
        else
        {
            controller.minMoveDistance = 0.1f; // Avoid huge values like 0.901f which stop movement
            activeCharacter.GetComponent<Animator>().Play("Breathing Idle");
        }
    }
}