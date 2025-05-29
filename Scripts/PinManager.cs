using UnityEngine;

public class PinManager : MonoBehaviour
{
    public static PinManager Instance;

    private int pinsDerribados = 0;

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

    public void PinDerribado()
    {
        pinsDerribados++;
        Debug.Log($"Pin derribado! Total este turno: {pinsDerribados}");
    }

    public void CalcularYSumarPuntos()
    {
        if (pinsDerribados > 0 && GameManager.Instance != null)
        {
            int puntos = GameManager.Instance.CalculatePoints(pinsDerribados);
            Debug.Log($"Enviando {puntos} puntos al GameManager (por {pinsDerribados} pins)");
            GameManager.Instance.AddScore(puntos);
        }
        pinsDerribados = 0;
    }
}