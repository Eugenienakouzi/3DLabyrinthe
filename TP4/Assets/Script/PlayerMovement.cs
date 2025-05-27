using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6f;

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Q/D
        float vertical = Input.GetAxis("Vertical");     // Z/S

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Ne rien faire s'il n'y a pas de direction
        if (direction.magnitude >= 0.1f)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Appliquer le mouvement
            Vector3 move = transform.TransformDirection(direction) * currentSpeed * Time.deltaTime;
            controller.Move(move);

            // Déclencher animation
            animator.SetFloat("Speed", currentSpeed);
        }
        else
        {
            // Pas de mouvement => Idle
            animator.SetFloat("Speed", 0f);
        }
    }
}
