using UnityEngine;
using UnityEngine.InputSystem;

public class RositaCharacterController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody RositaRB;
    public Transform CameraTransform;
    public float MoveSpeed = 5f;
    public float RotationSpeed = 10f;
    public float GroundThreshold = 1f;
    public float JumpForce = 1f;

    Vector3 forward;
    Vector3 right;
    Vector3 Direction;

    float speedMultiplier = 1f;
    bool isPunching, hasJumped, isInAir;

    void Update()
    {
        //salva l'informazione del corrente stato di animazione nella variabile state
        AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);

        //assegna a isPunching true se siamo nell'animazione dei pugni
        isPunching = state.IsName("Punch");

        //animazione dei pugni
        if (Input.GetMouseButtonDown(0) && !isPunching)
        {
            Animator.SetTrigger("Punch");
        }

        #region Movimento
        //variabili di movimento asse X e Z
        float Horizontal = 0f;
        float Vertical = 0f;

        //calcolo di forward e right
        forward = CameraTransform.forward;
        right = CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //assegna i vari valori a Vertical e Horizontal in base al tasto premuto
        if (Input.GetKey(KeyCode.W))
            Vertical = 1f;

        if (Input.GetKey(KeyCode.S))
            Vertical = -1f;

        if (Input.GetKey(KeyCode.A))
            Horizontal = -1f;

        if (Input.GetKey(KeyCode.D))
            Horizontal = 1f;

        //calcolo di isMoving e isRunning
        bool isMoving = Mathf.Abs(Horizontal) > 0.01f || Mathf.Abs(Vertical) > 0.01f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        //metti un moltiplicatore per la velocità in base a cosa sta facendo il giocatore
        speedMultiplier = isPunching ? 0 : isRunning ? 1.5f : 1;

        //metti le animazioni corrette in base alla modalità di movimento del giocatore
        Animator.SetBool("SoftRun", isMoving && !isRunning);
        Animator.SetBool("SpeedRun", isRunning);

        //calcolo della direzione di movimento
        Direction = (forward * Vertical) + (right * Horizontal);
        Direction.Normalize();

        //calcolo e applicazione della velocità di movimento
        Vector3 velocity = Direction * MoveSpeed * speedMultiplier;
        RositaRB.linearVelocity = new Vector3(velocity.x, RositaRB.linearVelocity.y, velocity.z);

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        #endregion

        //controllo del salto
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            isInAir = true;
        }
    }

    private void FixedUpdate()
    {
        //fisica del salto
        if (hasJumped)
        {
            RositaRB.AddForce(0, JumpForce, 0);
            hasJumped = false;
        }
    }

    bool isGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.distance > GroundThreshold)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}