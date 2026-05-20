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


   

    void Start()
    {

    }

    
    void Update()
    {

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




        if (Horizontal != 0 && Vertical != 0)
        {
            animator.SetBool("SoftRun", true);
            Direction = (forward * Vertical/2) + (right * Horizontal/2);

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
        Rosita.transform.Translate(Direction * force * Time.deltaTime, Space.World);

        if (forward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(Rosita.transform.rotation, rotation, 720 * Time.deltaTime);
        }

        

    }

}