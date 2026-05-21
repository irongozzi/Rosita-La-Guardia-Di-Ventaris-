using UnityEngine;

public class RositaCharacterController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody Rosita;
    public float MoveSpeed = 5f;
    public Transform CameraTransform;
    public float RotationSpeed = 10f;

    Vector3 forward;
    Vector3 right;
    Vector3 Direction;

    float speedMultiplier = 1f;
    bool pugni;

    void Update()
    {
        forward = CameraTransform.forward;
        right = CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("punch");
        }

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        pugni = state.IsName("Punch");

        float Horizontal = 0f;
        float Vertical = 0f;

        if (Input.GetKey(KeyCode.W))
            Vertical = 1f;

        if (Input.GetKey(KeyCode.S))
            Vertical = -1f;

        if (Input.GetKey(KeyCode.A))
            Horizontal = -1f;

        if (Input.GetKey(KeyCode.D))
            Horizontal = 1f;

        bool isMoving = Horizontal != 0 || Vertical != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        if (pugni)
        {
            speedMultiplier = 0.2f;
        }
        else if (isRunning)
        {
            speedMultiplier = 1.5f;
        }
        else
        {
            speedMultiplier = 1f;
        }

        animator.SetBool("SoftRun", isMoving && !isRunning);
        animator.SetBool("SpeedRun", isRunning);

        Direction = (forward * Vertical) + (right * Horizontal);
        Direction.Normalize();

        Vector3 velocity = Direction * MoveSpeed * speedMultiplier;

        Rosita.linearVelocity = new Vector3(
            velocity.x,
            Rosita.linearVelocity.y,
            velocity.z
        );

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );
        }
    }
}