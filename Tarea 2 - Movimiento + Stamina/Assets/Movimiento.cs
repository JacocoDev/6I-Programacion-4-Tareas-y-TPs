using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    public UIManager UIManager;
    public Slider slider;
    private float velocidad = 0.1f;
    private float energia = 1f;
    private float gastoEnergia = 0.001f;

    private void Awake()
    {
        slider.value = energia;
    }

    void Update()
    {
        if (UIManager.paused == false & energia >= 0)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * velocidad);
                energia -= gastoEnergia;
            }
            else if(Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * velocidad);
                energia -= gastoEnergia;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * velocidad);
                energia -= gastoEnergia;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * velocidad);
                energia -= gastoEnergia;
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                transform.Translate(Vector3.back * velocidad);
                energia -= gastoEnergia;
            }
            else if(Input.GetKey(KeyCode.E))
            {
                transform.Translate(Vector3.forward * velocidad);
                energia -= gastoEnergia;
            }
            else
            {
                Debug.Log("Esta tecla no tiene funcion");
            }

            slider.value = energia;
        }
    }
}