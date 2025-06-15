using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float mouseSensitivity = 100f;

    private CharacterController controller;
    private Animator animator;

    private float rotationY = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Tourner le joueur avec la souris (axe Y)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // Déplacement dans la direction avant du joueur
        float horizontal = Input.GetAxis("Horizontal"); // Q/D
        float vertical = Input.GetAxis("Vertical");     // Z/S

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            Vector3 move = transform.TransformDirection(direction) * currentSpeed * Time.deltaTime;
            controller.Move(move);

            animator.SetFloat("Speed", currentSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}
