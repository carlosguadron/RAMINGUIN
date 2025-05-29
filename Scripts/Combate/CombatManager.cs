using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Configuración")]
    public float delayInicioCombate = 1.5f;
    public float delayEntreTurnos = 1f;
    public int danioEnemigoPorTurno = 10; // Daño fijo que hace el enemigo cada turno

    [Header("Referencias del Jugador")]
    public PlayerControllerRaminguin playerController;
    public Camera camaraPrimeraPersona;
    public Camera camaraTerceraPersona;

    [Header("Sistema de Combate")]
    public GameObject panelCombate;
    public List<Camera> camarasCombate;
    private Camera camaraCombateActiva;
    public bool enCombate { get; private set; }

    [Header("Posiciones")]
    public Transform puntoTeletransportePrincipal;

    [Header("Eventos")]
    public UnityEvent OnCombateIniciado;
    public UnityEvent OnCombateFinalizado;

    [Header("UI de Resultado")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;

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

    public void IniciarCombate(int indiceCamaraCombate = 0)
    {
        if (indiceCamaraCombate < 0 || indiceCamaraCombate >= camarasCombate.Count)
        {
            Debug.LogError("Índice de cámara de combate no válido");
            return;
        }

        enCombate = true;

        if (playerController != null)
        {
            playerController.enabled = false;
            playerController.SetControlActive(false);
            camaraPrimeraPersona.gameObject.SetActive(false);
            camaraTerceraPersona.gameObject.SetActive(false);
        }

        camaraCombateActiva = camarasCombate[indiceCamaraCombate];
        camaraCombateActiva.gameObject.SetActive(true);
        panelCombate.SetActive(true);

        StartCoroutine(SequenciaInicioCombate());
    }

    IEnumerator SequenciaInicioCombate()
    {
        OnCombateIniciado?.Invoke();
        TurnUI.Instance.UpdateTurnUI(true);
        yield return new WaitForSeconds(delayInicioCombate);
        GameManager.Instance.PrepararTurnoJugador();
    }

    public void CambiarTurno(bool turnoJugador)
    {
        StartCoroutine(SequenciaCambioTurno(turnoJugador));
    }

    IEnumerator SequenciaCambioTurno(bool turnoJugador)
    {
        TurnUI.Instance.UpdateTurnUI(turnoJugador);
        yield return new WaitForSeconds(delayEntreTurnos);

        if (turnoJugador)
        {
            GameManager.Instance.PrepararTurnoJugador();
        }
        else
        {
            // En lugar de iniciar turno enemigo, aplicamos daño directamente
            GameManager.Instance.AplicarDañoJugador(danioEnemigoPorTurno);
            yield return new WaitForSeconds(1f); // Espera para ver el daño

            // Volvemos al turno del jugador
            CambiarTurno(true);
        }
    }

    public void FinalizarCombate(bool victoria)
    {
        StartCoroutine(SequenciaFinCombate(victoria));
    }

    IEnumerator SequenciaFinCombate(bool victoria)
    {
        yield return new WaitForSeconds(1f);

        if (camaraCombateActiva != null)
            camaraCombateActiva.gameObject.SetActive(false);

        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.SetControlActive(true);
            playerController.RestaurarCamaras();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        panelCombate.SetActive(false);
        enCombate = false;
        OnCombateFinalizado?.Invoke();

        if (victoria) GameManager.Instance.TerminarCombate(true);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AgregarCamaraCombate(Camera nuevaCamara)
    {
        if (!camarasCombate.Contains(nuevaCamara))
        {
            camarasCombate.Add(nuevaCamara);
            nuevaCamara.gameObject.SetActive(false);
        }
    }

    public void MostrarResultado(bool victoria)
    {
        if (victoria)
        {
            panelVictoria?.SetActive(true);
            StartCoroutine(TeletransportarDespuesDe(3f));
        }
        else
        {
            panelDerrota?.SetActive(true);
            StartCoroutine(ReiniciarDespuesDe(3f));
        }
    }

    IEnumerator TeletransportarDespuesDe(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        // Desactivar todos los elementos de UI primero
        panelVictoria?.SetActive(false);
        panelCombate.SetActive(false);
        TurnUI.Instance?.gameObject.SetActive(false);

        // Restaurar al jugador completamente
        if (playerController != null)
        {
            playerController.transform.position = puntoTeletransportePrincipal.position;
            playerController.enabled = true;
            playerController.SetControlActive(true);
            playerController.RestaurarCamaras();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Desactivar cámara de combate
        if (camaraCombateActiva != null)
            camaraCombateActiva.gameObject.SetActive(false);

        // Reactivar cámara normal del jugador
        camaraPrimeraPersona.gameObject.SetActive(true);
        camaraTerceraPersona.gameObject.SetActive(true);

        // Resetear estado del combate
        enCombate = false;
        GameManager.Instance.ResetearCombateCompleto();
    }

    IEnumerator ReiniciarDespuesDe(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    // Agrega este método:
    public void PanelTurnosCompletado()
    {
        if (enCombate)
        {
            if (GameManager.Instance.turnoJugador) // Acceder a través de GameManager
            {
                GameManager.Instance.PrepararTurnoJugador();
            }
            else
            {
                GameManager.Instance.AplicarDañoJugador(danioEnemigoPorTurno);
                StartCoroutine(CambiarATurnoJugadorConDelay(1f));
            }
        }
    }

    IEnumerator CambiarATurnoJugadorConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CambiarTurno(true);
    }

}