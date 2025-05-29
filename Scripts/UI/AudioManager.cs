using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración de Audio")]
    public AudioMixer mixer;
    public AudioSource musicaFondo;
    public AudioClip[] canciones;

    [Range(0f, 1f)] public float volumenMusica = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Cargar configuración guardada
            volumenMusica = PlayerPrefs.GetFloat("VolumenMusica", 0.7f);
            ActualizarVolumenes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActualizarVolumenes()
    {
        // Actualiza ambos sistemas:
        // Mixer (si lo usas)
        if (mixer != null)
        {
            mixer.SetFloat("VolumenMusica", Mathf.Log10(volumenMusica) * 20);
        }

        // AudioSource directo
        if (musicaFondo != null)
        {
            musicaFondo.volume = volumenMusica;
        }

        PlayerPrefs.SetFloat("VolumenMusica", volumenMusica);
    }

    public void PausarMusica(bool pausar)
    {
        if (musicaFondo == null) return;

        if (pausar) musicaFondo.Pause();
        else musicaFondo.UnPause();
    }
}