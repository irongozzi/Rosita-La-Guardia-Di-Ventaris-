using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RositaCharacterController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody Rosita;
    public float force = 1;
    private float timers = 0f;
    public Transform CameraTransform;
    Vector3 forward;
    Vector3 right;
    Vector3 Direction;
    public AudioSource FootStep;
    public float speed = 100f;
    private bool Corsa;
    public float RotationSpeed = 10f;
    float speedMultiplier = 1f;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("SoftRun", false);
            animator.SetBool("TrueRun", false);
            animator.SetTrigger("punch");
            

        }


        float Horizontal = 0f;
        float Vertical = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            Vertical = 1f;
            animator.SetBool("SoftRun", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vertical = -1f;
            animator.SetBool("SoftRun", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Horizontal = -1f;
            animator.SetBool("SoftRun", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Horizontal = 1f;
            animator.SetBool("SoftRun", true);
        }

        if ((Horizontal != 0 || Vertical != 0) && Input.GetKey(KeyCode.LeftShift))
        {
            print("ok");
            animator.SetBool("SoftRun", false);
            animator.SetBool("TrueRun", true);
            Direction = (forward * Vertical * 2) + (right * Horizontal * 2);
            speedMultiplier = 1.5f;
        }
        else
        {
            animator.SetBool("SoftRun", true);
            animator.SetBool("TrueRun", false);
            speedMultiplier = 1f;
        }

        if (Horizontal != 0 && Vertical != 0)
        {
            animator.SetBool("SoftRun", true);
            Direction = (forward * Vertical / 3) + (right * Horizontal / 3);
        }
        else
        {
            Corsa = false;
        }

        if (Horizontal != 0 || Vertical != 0)
        {
            animator.SetBool("SoftRun", true);
        }
        else
        {
            animator.SetBool("SoftRun", false);
        }

        forward = CameraTransform.forward;
        right = CameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Direction = (forward * Vertical) + (right * Horizontal);
        Rosita.transform.Translate(Direction * force * speedMultiplier * Time.deltaTime, Space.World);

        if (Horizontal != 0 || Vertical != 0)
        {
            Vector3 moveDirection = Direction.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }
}