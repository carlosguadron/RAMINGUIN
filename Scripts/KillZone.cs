using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{
    public Transform puntoReinicioJugador;
    public float delayReinicio = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;
        if (!CombatManager.Instance.enCombate) return;

        StartCoroutine(ManejarFinTurno(other.gameObject));
    }

    IEnumerator ManejarFinTurno(GameObject bola)
    {
        PinManager.Instance?.CalcularYSumarPuntos();
        yield return new WaitForSeconds(delayReinicio);

        bola.transform.position = puntoReinicioJugador.position;
        GameManager.Instance.EndTurn();
    }
}