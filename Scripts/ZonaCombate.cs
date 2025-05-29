using UnityEngine;

public class ZonaCombate : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject zonaCombateUI;
    public MonoBehaviour shooterScript;
    public GameObject scorePanel;
    public int indiceCamaraCombate = 0;

    [Header("Configuración")]
    public bool reiniciarAlSalir = false;

    private bool triggered = false;

    private void Start()
    {
        if (zonaCombateUI != null) zonaCombateUI.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(false);

        if (TryGetComponent(out Camera camaraCombate))
        {
            CombatManager.Instance.AgregarCamaraCombate(camaraCombate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        CombatManager.Instance.IniciarCombate(indiceCamaraCombate); // Cambiado a usar directamente CombatManager

        if (zonaCombateUI != null) zonaCombateUI.SetActive(true);
        if (scorePanel != null) scorePanel.SetActive(true);

        if (shooterScript != null)
        {
            var method = shooterScript.GetType().GetMethod("ActivarCombate");
            method?.Invoke(shooterScript, null);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!reiniciarAlSalir || !triggered || !other.CompareTag("Player")) return;

        triggered = false;

        // Desactivar todos los elementos de la zona de combate
        if (zonaCombateUI != null) zonaCombateUI.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(false);

        // Resetear el shooter si existe
        if (shooterScript != null)
        {
            var resetMethod = shooterScript.GetType().GetMethod("ResetearTodo");
            resetMethod?.Invoke(shooterScript, null);
        }
    }
}