using UnityEngine;
using UnityEngine.Rendering;

public class Movimiento : MonoBehaviour
{
    public UIManager UIManager;
    private float velocidad = 0.1f;
    private void Awake()
    {
        Debug.Log("Soy el Awake");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Soy el Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.paused == false)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * velocidad);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * velocidad);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * velocidad);
            }
            else if(Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * velocidad);
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                transform.Translate(Vector3.back * velocidad);
            }
            else if(Input.GetKey(KeyCode.E))
            {
                transform.Translate(Vector3.forward * velocidad);
            }
            else
            {
                Debug.Log("Esta tecla no tiene funcion");
            }
        }
    }
}
