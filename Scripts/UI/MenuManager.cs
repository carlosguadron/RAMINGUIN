using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelConfiguration;
    public GameObject panelPausa;

    private bool juegoPausado = false;

    void Awake()
    {
        // Asegurar que el menú principal es lo primero que se muestra
        MostrarMenuPrincipal();
    }

    void Start()
    {
        // Forzar estado inicial
        SetCursorState(true);
        Time.timeScale = 0f; // Pausar el juego al inicio
    }

    void Update()
    {
        // Manejar tecla ESC para pausar/continuar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelMenuPrincipal.activeSelf || panelConfiguration.activeSelf) return;

            if (juegoPausado) ContinuarJuego();
            else MostrarPausa();
        }
    }

    public void Jugar()
    {
        panelMenuPrincipal.SetActive(false);
        SetCursorState(false);
        Time.timeScale = 1f; // Reanudar el juego
        juegoPausado = false;

        AudioManager.Instance?.PausarMusica(false);
    }

    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelConfiguration.SetActive(false);
        panelPausa.SetActive(false);
        SetCursorState(true);
        Time.timeScale = 0f; // Pausar el juego
        juegoPausado = true;
    }

    public void MostrarConfiguracion()
    {
        panelMenuPrincipal.SetActive(false);
        panelConfiguration.SetActive(true);
        panelPausa.SetActive(false);
        Time.timeScale = 0f;
    }

    public void MostrarPausa()
    {
        panelPausa.SetActive(true);
        GameManager.Instance?.SetCursorState(true);
        AudioManager.Instance?.PausarMusica(true);
        Time.timeScale = 0f;
    }

    public void ContinuarJuego()
    {
        panelPausa.SetActive(false);
        GameManager.Instance?.SetCursorState(false);
        AudioManager.Instance?.PausarMusica(false);
        Time.timeScale = 1f;
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