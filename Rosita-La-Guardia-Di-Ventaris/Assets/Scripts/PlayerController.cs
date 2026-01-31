using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float camRotationSpeedX = 1f;
    public float camRotationSpeedY = 1f;
    public GameObject view;

    float camRotationX, camRotationY;
    float horizontalAxis, verticalAxis;
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
        horizontalAxis = Input.GetAxis("Horizontal") * movementSpeed;
        Vector3 calculatedStrafeVelocity = transform.right * horizontalAxis;

        //calcolo del movimento sull'asse verticale (movimento avanti-indietro, funzionante sia su M&K che su controller)
        verticalAxis = Input.GetAxis("Vertical") * movementSpeed;
        Vector3 calculatedLinearVelocity = transform.forward * verticalAxis;

        //applicazione dei due movimenti calcolati
        Vector3 calculatedTotalVelocity = calculatedStrafeVelocity + calculatedLinearVelocity;
        playerRB.linearVelocity = new Vector3(calculatedTotalVelocity.x, playerRB.linearVelocity.y, calculatedTotalVelocity.z);

        //calcolo e applicazione della rotazione della visuale in su e in giù (funzionante sia su M&K che su controller)
        camRotationX = Mathf.Clamp(Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal Secondary"), -1, 1);
        transform.Rotate(0, (camRotationX * camRotationSpeedX) * Time.deltaTime, 0);

        camRotationY = Mathf.Clamp(Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical Secondary"), -1, 1);
        view.transform.Rotate((-camRotationY * camRotationSpeedY) * Time.deltaTime, 0, 0);
    }
}
