using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    public static MusicaFondo instancia;
    private AudioSource audioSource;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            // Asignar volumen desde PlayerPrefs
            float volumenGuardado = PlayerPrefs.GetFloat("volumen", 0.5f); // valor por defecto 0.5
            audioSource.volume = volumenGuardado;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        if (audioSource != null)
        {
            audioSource.volume = nuevoVolumen;
            PlayerPrefs.SetFloat("volumen", nuevoVolumen);  // Lo guarda automáticamente
        }
    }

    public void DetenerMusica()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
