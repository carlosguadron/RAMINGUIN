using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject panelMenuPrincipal;
    public GameObject panelConfiguration;
    public GameObject panelPausa;
    public TextMeshProUGUI textoBotonVolver; // Referencia al texto del bot�n Volver

    [Header("Configuraci�n")]
    public string textoVolverPausa = "Continuar";
    public string textoVolverMenu = "Men� Principal";

    private bool juegoPausado = false;
    private bool configDesdePausa = false;

    void Awake()
    {
        // Asegurar que solo mostramos el men� principal al inicio
        MostrarMenuPrincipal();
    }

    void Start()
    {
        // Configuraci�n inicial
        SetCursorState(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        // Manejo de la tecla ESC en todo momento
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelConfiguration.activeSelf)
            {
                // Si estamos en configuraci�n, volver al men� anterior
                VolverConfiguracion();
            }
            else if (panelPausa.activeSelf)
            {
                // Si estamos en pausa, continuar el juego
                ContinuarJuego();
            }
            else if (!panelMenuPrincipal.activeSelf)
            {
                // Si estamos jugando (sin men�s activos), pausar el juego
                MostrarPausa();
            }
            // Si el men� principal est� activo, ESC no hace nada
        }
    }

    public void ActualizarTextoBotonVolver()
    {
        if (textoBotonVolver != null)
        {
            textoBotonVolver.text = configDesdePausa ? textoVolverPausa : textoVolverMenu;
        }
    }

    public void VolverConfiguracion()
    {
        if (configDesdePausa)
        {
            MostrarPausa();
        }
        else
        {
            MostrarMenuPrincipal();
        }
    }

    public void Jugar()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguration.SetActive(false);
        panelPausa.SetActive(false);

        SetCursorState(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        AudioManager.Instance?.PausarMusica(false);
    }

    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelConfiguration.SetActive(false);
        panelPausa.SetActive(false);

        SetCursorState(true);
        Time.timeScale = 0f;
        juegoPausado = true;
        configDesdePausa = false;

        ActualizarTextoBotonVolver();
    }

    public void MostrarConfiguracion()
    {
        // Detectamos de d�nde venimos
        configDesdePausa = panelPausa.activeSelf;

        panelMenuPrincipal.SetActive(false);
        panelConfiguration.SetActive(true);
        panelPausa.SetActive(false);

        Time.timeScale = 0f;

        ActualizarTextoBotonVolver();
    }

    public void MostrarPausa()
    {
        panelPausa.SetActive(true);
        panelConfiguration.SetActive(false);
        panelMenuPrincipal.SetActive(false);

        GameManager.Instance?.SetCursorState(true);
        AudioManager.Instance?.PausarMusica(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void ContinuarJuego()
    {
        panelPausa.SetActive(false);
        panelConfiguration.SetActive(false);
        panelMenuPrincipal.SetActive(false);

        GameManager.Instance?.SetCursorState(false);
        AudioManager.Instance?.PausarMusica(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void Salir()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetCursorState(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }
}