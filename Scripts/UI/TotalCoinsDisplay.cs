using TMPro;
using UnityEngine;

public class TotalCoinsDisplay : MonoBehaviour
{
    public static TotalCoinsDisplay Instance;
    public TMP_Text totalCoinsText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTotalCoins(int total)
    {
        if (totalCoinsText != null)
        {
            totalCoinsText.text = total.ToString();
            Debug.Log("Monedas totales actualizadas: " + total);
        }
    }
}