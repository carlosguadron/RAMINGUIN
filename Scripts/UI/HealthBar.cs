using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;

    public void UpdateHealth(float normalizedValue)
    {
        if (fillImage != null)
        {
            Debug.Log($"Actualizando health bar a {normalizedValue * 100}%");
            fillImage.fillAmount = normalizedValue;

            // Cambia color según salud (opcional)
            fillImage.color = Color.Lerp(Color.red, Color.green, normalizedValue);
        }
        else
        {
            Debug.LogError("Fill Image no asignada en HealthBar!");
        }
    }
}