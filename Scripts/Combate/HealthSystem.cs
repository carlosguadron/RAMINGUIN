using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Configuración")]
    public bool esJugador = false; // Nueva variable para identificar si es el jugador

    [Header("Eventos")]
    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;

    [Header("UI Reference")]
    public HealthBar healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            OnHealthChanged.AddListener(healthBar.UpdateHealth);
        }

        NotifyHealthChange();
    }

    public void ApplyDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} recibió {damage} de daño. Vida: {currentHealth}/{maxHealth}");
        NotifyHealthChange();

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} derrotado!");
            OnDeath.Invoke();

            // Notificar al CombatManager si existe
            if (CombatManager.Instance != null)
            {
                CombatManager.Instance.MostrarResultado(!esJugador);
            }
        }
    }

    private void NotifyHealthChange()
    {
        float normalizedHealth = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(normalizedHealth);

        // Actualización directa como respaldo
        if (healthBar != null)
        {
            healthBar.UpdateHealth(normalizedHealth);
        }
    }

    public int GetCurrentHealth() => currentHealth;
}