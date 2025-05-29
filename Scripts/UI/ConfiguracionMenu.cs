using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionMenu : MonoBehaviour
{
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderSFX;
    public Toggle togglePantallaCompleta;

    private void Start()
    {
        // Cargar valores guardados
        sliderGeneral.value = PlayerPrefs.GetFloat("VolumenGeneral", 1f);
        sliderMusica.value = PlayerPrefs.GetFloat("VolumenMusica", 1f);
        sliderSFX.value = PlayerPrefs.GetFloat("VolumenSFX", 1f);
        togglePantallaCompleta.isOn = Screen.fullScreen;

        ActualizarVolumenes();
    }

    public void ActualizarVolumenes()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.volumenGeneral = sliderGeneral.value;
            AudioManager.Instance.volumenMusica = sliderMusica.value;
            AudioManager.Instance.volumenSFX = sliderSFX.value;
            AudioManager.Instance.ActualizarVolumenes();
        }

        // Guardar preferencias
        PlayerPrefs.SetFloat("VolumenGeneral", sliderGeneral.value);
        PlayerPrefs.SetFloat("VolumenMusica", sliderMusica.value);
        PlayerPrefs.SetFloat("VolumenSFX", sliderSFX.value);
    }

    public void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompleta ? 1 : 0);
    }

    public void RestablecerValores()
    {
        sliderGeneral.value = 1f;
        sliderMusica.value = 1f;
        sliderSFX.value = 1f;
        togglePantallaCompleta.isOn = true;
        ActualizarVolumenes();
    }
}