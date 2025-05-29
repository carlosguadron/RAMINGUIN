using UnityEngine;
using TMPro;
using System.Collections;

public class TurnUI : MonoBehaviour
{
    public static TurnUI Instance;

    [Header("Configuración")]
    public float tiempoMostrarMensaje = 2f;
    public float fadeDuration = 0.5f;
    public float delayDespuesTexto = 2f; // Nuevo: tiempo de espera después de ocultar

    [Header("Referencias")]
    public GameObject panelTurnos;
    public TMP_Text textTurnoPlayer;
    public TMP_Text textTurnoEnemy;
    public CanvasGroup canvasGroup;

    private bool panelActivo = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            panelTurnos.SetActive(false);
        }
    }

    public void UpdateTurnUI(bool isPlayerTurn)
    {
        // Solo mostrar el panel si estamos en combate
        if (CombatManager.Instance.enCombate)
        {
            textTurnoPlayer.gameObject.SetActive(isPlayerTurn);
            textTurnoEnemy.gameObject.SetActive(!isPlayerTurn);
            StartCoroutine(MostrarPanel());
        }
        else
        {
            // Si no estamos en combate, ocultar todo
            panelTurnos.SetActive(false);
        }
    }

    IEnumerator MostrarPanel()
    {
        panelActivo = true;

        // Bloquear controles
        GameManager.Instance.SetCursorState(true);
        if (GameManager.Instance.playerController != null)
            GameManager.Instance.playerController.SetControlActive(false);

        // Mostrar panel con fade in
        panelTurnos.SetActive(true);
        canvasGroup.alpha = 0;

        float timer = 0;
        while (timer < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(tiempoMostrarMensaje);

        // Ocultar panel con fade out
        timer = 0;
        while (timer < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        panelTurnos.SetActive(false);
        yield return new WaitForSeconds(delayDespuesTexto); // Espera adicional

        panelActivo = false;

        // Notificar que el panel terminó
        CombatManager.Instance.PanelTurnosCompletado();
    }

    public bool IsPanelActive() => panelActivo;
}