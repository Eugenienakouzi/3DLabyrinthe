using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float mouseSensitivity = 100f;
    public float gravity = -9.81f;

    public CharacterController controller;
    private Animator animator;

    private float rotationY = 0f;
    private Vector3 velocity; // Pour la gravité

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotatePlayer();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float speed = 0f;
        if (direction.magnitude >= 0.1f)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            speed = isRunning ? runSpeed : walkSpeed;

            Vector3 move = transform.TransformDirection(direction) * speed;
            controller.Move(move * Time.deltaTime);
        }

        // Appliquer la gravité
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Petite valeur négative pour rester bien collé au sol
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", speed);
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
