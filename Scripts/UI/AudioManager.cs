using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración de Audio")]
    public AudioMixer mixer;
    public AudioSource musicaFondo;
    public AudioClip[] canciones;

    [Header("Volúmenes")]
    [Range(0f, 1f)] public float volumenGeneral = 1f;
    [Range(0f, 1f)] public float volumenMusica = 1f;
    [Range(0f, 1f)] public float volumenSFX = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ActualizarVolumenes();
        ReproducirMusica(0); // Reproduce la primera canción
    }

    public void ReproducirMusica(int indice)
    {
        if (indice >= 0 && indice < canciones.Length)
        {
            musicaFondo.clip = canciones[indice];
            musicaFondo.Play();
        }
    }

    public void ActualizarVolumenes()
    {
        // Convierte el valor lineal (0-1) a logarítmico (-80 a 0 dB)
        mixer.SetFloat("VolumenGeneral", Mathf.Log10(volumenGeneral) * 20);
        mixer.SetFloat("VolumenMusica", Mathf.Log10(volumenMusica) * 20);
        mixer.SetFloat("VolumenSFX", Mathf.Log10(volumenSFX) * 20);
    }

    public void PausarMusica(bool pausar)
    {
        if (pausar) musicaFondo.Pause();
        else musicaFondo.UnPause();
    }

    public void CambiarCancion(AudioClip nuevaCancion)
    {
        musicaFondo.clip = nuevaCancion;
        musicaFondo.Play();
    }
}