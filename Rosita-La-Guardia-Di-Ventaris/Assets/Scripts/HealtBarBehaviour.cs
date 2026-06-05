using UnityEngine;
using UnityEngine.UI;

public class HealtBarBehaviour : MonoBehaviour
{
    public Image healthFill;
    
    public float health = 0.0005f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            healthFill.fillAmount -= health;
        }
        
    }
}
