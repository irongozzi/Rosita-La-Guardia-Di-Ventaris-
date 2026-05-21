using UnityEngine;

public class VisualRositaBehaviour : MonoBehaviour
{
    [Header("Sensibilità Mouse")]
    public float MouseSensitivityY = 200f;
    public float MouseSensitivityX = 200f;


    [Header("Corpo del Player")]
    public Transform PlayerBody;

    // Rotazione verticale camera
    float xRotation;

    // Limiti visuale verticale
    public float TopClamp = 10f;
    public float BottomClamp = -5f;

    void Start()
    {
        // Blocca il cursore al centro
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        // Input mouse
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivityY * Time.deltaTime;

        // Movimento verticale camera
        xRotation -= mouseY;

        // Limiti visuale
        xRotation = Mathf.Clamp(xRotation, BottomClamp, TopClamp);

        // Applica rotazione verticale
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotazione orizzontale player
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}