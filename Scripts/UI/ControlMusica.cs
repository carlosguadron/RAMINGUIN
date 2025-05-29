using UnityEngine;
using UnityEngine.UI;

public class ControlMusica : MonoBehaviour
{
    public Slider sliderMusica;

    void Start()
    {
        // Configurar el slider con el valor actual
        sliderMusica.value = GameManager.Instance.volumenMusica;
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        // Actualizar el volumen en el GameManager
        GameManager.Instance.volumenMusica = nuevoVolumen;
        GameManager.Instance.ActualizarVolumenMusica();
    }
}