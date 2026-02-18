using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float runSpeed = 1f;
    public float camRotationSpeedX = 1f;
    public float camRotationSpeedY = 1f;
    public float jumpForce = 1f;
    public float groundThreshold = 1f;
    public float camRotationRange;
    public GameObject view;

    float calculatedCamRotationX, calculatedCamRotationY;
    float camRotationX, camRotationY;
    float horizontalAxis, verticalAxis;
    bool jumpIsPressed;
    Vector3 calculatedStrafeVelocity, calculatedLinearVelocity;
    Rigidbody playerRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //setto in automatico il RigidBody della capsula
        playerRB = GetComponent<Rigidbody>();

        //nascondo il cursore e lo blocco al centro dello schermo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //calcolo del movimento sull'asse orizzontale (strafe, funzionante sia su M&K che su controller)
        horizontalAxis = Input.GetAxis("Horizontal") * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : movementSpeed);
        calculatedStrafeVelocity = transform.right * horizontalAxis;

        //calcolo del movimento sull'asse verticale (movimento avanti-indietro, funzionante sia su M&K che su controller)
        verticalAxis = Input.GetAxis("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? runSpeed : movementSpeed);
        calculatedLinearVelocity = transform.forward * verticalAxis;

        //calcolo e applicazione della rotazione della visuale in su e in giù (funzionante sia su M&K che su controller)
        camRotationX = Mathf.Clamp(Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal Secondary"), -1, 1);
        calculatedCamRotationX = (camRotationX * camRotationSpeedX) * Time.deltaTime;
        transform.Rotate(0, calculatedCamRotationX, 0);

        camRotationY = Mathf.Clamp(Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical Secondary"), -1, 1); //prendo l'input del player normalizzato
        calculatedCamRotationY += (camRotationY * camRotationSpeedY) * Time.deltaTime; //calcolo la rotazione del giocatore 
        calculatedCamRotationY = Mathf.Clamp(calculatedCamRotationY, -camRotationRange, camRotationRange);
        view.transform.localEulerAngles = new Vector3(calculatedCamRotationY, 0, 0); //applicazione della rotazione agli angoli di eulero della visuale

        //salto - input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpIsPressed = true;
        }
    }

    void FixedUpdate()
    {
        //applicazione dei due movimenti calcolati
        Vector3 calculatedTotalVelocity = calculatedStrafeVelocity + calculatedLinearVelocity;
        playerRB.linearVelocity = new Vector3(calculatedTotalVelocity.x, playerRB.linearVelocity.y, calculatedTotalVelocity.z);

        //salto - fisica
        if (jumpIsPressed)
        {
            playerRB.AddForce(transform.up * jumpForce);
            jumpIsPressed = false;
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.distance < groundThreshold)
            {
                Debug.DrawRay(transform.position, -transform.up, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, -transform.up, Color.red);
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
