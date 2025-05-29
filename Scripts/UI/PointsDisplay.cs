using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    public TMP_Text pointsText;
    public float displayDuration = 2f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowPoints(int points)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        pointsText.text = $"+{points} puntos";
        gameObject.SetActive(true);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}