using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Configuración de Audio")]
    public AudioSource musicaFondo;

    [Range(0f, 1f)]
    public float volumenMusica = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Configuración inicial del AudioSource
            musicaFondo.volume = 0f; // Empieza en 0
            volumenMusica = PlayerPrefs.GetFloat("VolumenMusica", 0.7f);
            ActualizarVolumen();

            if (!musicaFondo.isPlaying)
                musicaFondo.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActualizarVolumen()
    {
        // Asegurarse que el volumen llega realmente a 0
        float volumenClamped = Mathf.Clamp(volumenMusica, 0f, 1f);

        musicaFondo.volume = volumenClamped;

        // Silenciar completamente si está en 0
        musicaFondo.mute = (volumenClamped <= 0.001f);

        PlayerPrefs.SetFloat("VolumenMusica", volumenClamped);
        PlayerPrefs.Save();
    }

    public void PausarMusica(bool pausar)
    {
        if (pausar) musicaFondo.Pause();
        else musicaFondo.UnPause();
    }
}