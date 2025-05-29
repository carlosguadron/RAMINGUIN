using UnityEngine;
using UnityEngine.UI;

public class ControlMusica : MonoBehaviour
{
    public Slider sliderMusica;

    void Start()
    {
        // Usar el valor de AudioManager en lugar de GameManager
        sliderMusica.value = AudioManager.Instance.volumenMusica;
        sliderMusica.onValueChanged.AddListener(CambiarVolumen);
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        AudioManager.Instance.volumenMusica = nuevoVolumen;
        AudioManager.Instance.ActualizarVolumenes();
    }
}