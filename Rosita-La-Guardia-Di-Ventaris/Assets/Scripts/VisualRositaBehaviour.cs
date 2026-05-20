using UnityEngine;

public class VisualRositaBehaviour : MonoBehaviour
{
    [Header("Sensibilità Mouse")]
    public float mouseSensitivityY = 200f;
    public float mouseSensitivityX = 200f;


    [Header("Corpo del Player")]
    public Transform playerBody;

    // Rotazione verticale camera
    public float xRotation = 1f;

    // Limiti visuale verticale
    public float topClamp = 10f;
    public float bottomClamp = -5f;

    void Start()
    {
        // Blocca il cursore al centro
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        // Input mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        // Movimento verticale camera
        xRotation -= mouseY;

        // Limiti visuale
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Applica rotazione verticale
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotazione orizzontale player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}