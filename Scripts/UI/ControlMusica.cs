using UnityEngine;
using UnityEngine.UI;

public class ControlMusica : MonoBehaviour
{
    [Header("Referencias")]
    public Slider sliderMusica;

    void Start()
    {
        // Configuración inicial del slider
        sliderMusica.minValue = 0f;
        sliderMusica.maxValue = 1f;
        sliderMusica.value = AudioManager.Instance.volumenMusica;

        // Asignar el evento
        sliderMusica.onValueChanged.RemoveAllListeners();
        sliderMusica.onValueChanged.AddListener(CambiarVolumen);
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.volumenMusica = nuevoVolumen;
            AudioManager.Instance.ActualizarVolumen();
        }
    }
}