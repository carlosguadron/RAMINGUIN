using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportDestination;

    [Header("Cámaras del jugador")]
    public GameObject firstPersonCamGO;  // Cambia a GameObject
    public GameObject thirdPersonCamGO;  // Cambia a GameObject

    [Header("Cámara de combate")]
    public GameObject combatCameraGO;     // GameObject para la cámara de combate

    public MonoBehaviour playerMovementScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Teletransporta al jugador
            other.transform.position = teleportDestination.position;

            // Desactiva las cámaras del jugador
            if (firstPersonCamGO != null) firstPersonCamGO.SetActive(false);
            if (thirdPersonCamGO != null) thirdPersonCamGO.SetActive(false);

            // Activa la cámara de combate
            if (combatCameraGO != null) combatCameraGO.SetActive(true);

            // Desactiva el movimiento del jugador (si quieres)
            if (playerMovementScript != null) playerMovementScript.enabled = false;

            // Ajusta el cursor para el combate (visible y desbloqueado)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
