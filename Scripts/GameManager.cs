using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TMP_Text scoreText;
    public PointsDisplay pointsDisplay;

    [Header("Game Systems")]
    public HealthSystem enemyHealthSystem;
    public HealthSystem playerHealthSystem;
    public BallShooter ballShooter;
    public PlayerControllerRaminguin playerController;

    [Header("Ball Settings")]
    public Transform puntoReinicioJugador;
    public Rigidbody pelota;

    [Header("Audio Settings")]
    public AudioSource musicaFondo;
    [Range(0f, 1f)] public float volumenMusica = 1f;

    private int score = 0;
    private int scoreTurno = 0;
    public bool turnoJugador = true;

    void Awake()
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

    void Start()
    {
        // Elimina cualquier SetCursorState del Start
        UpdateScoreUI();

        // Solo cargar volumen si no hay otro manager haciéndolo
        if (AudioManager.Instance == null)
        {
            volumenMusica = PlayerPrefs.GetFloat("VolumenMusica", 1f);
            ActualizarVolumenMusica();
        }
    }
    public void ActualizarVolumenMusica()
    {
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

    public void AddScore(int points)
    {
        scoreTurno += points;
        score += points;
        UpdateScoreUI();
        pointsDisplay?.ShowPoints(points);
        TotalCoinsDisplay.Instance?.UpdateTotalCoins(score);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntuación: " + score;
    }

    public void EndTurn()
    {
        // No hacer nada si el panel está activo
        if (TurnUI.Instance != null && TurnUI.Instance.IsPanelActive()) return;

        if (enemyHealthSystem != null && scoreTurno > 0)
        {
            enemyHealthSystem.ApplyDamage(scoreTurno);

            if (enemyHealthSystem.GetCurrentHealth() <= 0)
            {
                TerminarCombate(true);
                return;
            }
        }

        scoreTurno = 0;
        CombatManager.Instance.CambiarTurno(false);
    }

    public void PrepararTurnoJugador()
    {
        turnoJugador = true;
        ResetearBola(puntoReinicioJugador);
        ballShooter.ActivarCombate();
    }

    private void ResetearBola(Transform puntoReinicio)
    {
        pelota.transform.position = puntoReinicio.position;
        pelota.velocity = Vector3.zero;
        pelota.angularVelocity = Vector3.zero;
        pelota.isKinematic = true;
    }

    public void TerminarCombate(bool jugadorGana)
    {
        turnoJugador = true;
        scoreTurno = 0;

        if (jugadorGana)
        {
            playerController.DesactivarModoCombate();
            CombatManager.Instance?.FinalizarCombate(true); // Notificar al CombatManager
        }
        else
        {
            playerController.SetControlActive(true);
            playerController.RestaurarCamaras();
            CombatManager.Instance?.MostrarResultado(false);
        }
    }

    public void SetCursorState(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }

    public int CalculatePoints(int pinsHit)
    {
        return pinsHit <= 0 ? 0 : (pinsHit <= 4 ? 10 : 10 + (pinsHit - 4));
    }

    public void AplicarDañoJugador(int damage)
    {
        if (playerHealthSystem != null)
        {
            playerHealthSystem.ApplyDamage(damage);
            if (playerHealthSystem.GetCurrentHealth() <= 0)
            {
                TerminarCombate(false);
                CombatManager.Instance.MostrarResultado(false);
            }
        }
    }

    public void ResetearCombateCompleto()
    {
        turnoJugador = true;
        scoreTurno = 0;

        // Resetear la bola completamente
        if (pelota != null)
        {
            pelota.velocity = Vector3.zero;
            pelota.angularVelocity = Vector3.zero;
            pelota.isKinematic = true;
            pelota.gameObject.SetActive(false);
        }

        // Asegurar que el BallShooter está resetado
        if (ballShooter != null)
        {
            ballShooter.ResetearTodo();
        }
    }


}   