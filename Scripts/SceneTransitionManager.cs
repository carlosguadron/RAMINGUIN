using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TeleportWithTransition(Transform player, Vector3 destination, List<Camera> camerasToDisable, Camera cameraToEnable, MonoBehaviour playerControlScript)
    {
        StartCoroutine(DoTransition(player, destination, camerasToDisable, cameraToEnable, playerControlScript));
    }

    // Coroutine pública para que otros scripts puedan esperar el proceso
    public IEnumerator TeleportWithTransitionCoroutine(Transform player, Vector3 destination, List<Camera> camerasToDisable, Camera cameraToEnable, MonoBehaviour playerControlScript)
    {
        yield return DoTransition(player, destination, camerasToDisable, cameraToEnable, playerControlScript);
    }

    private IEnumerator DoTransition(Transform player, Vector3 destination, List<Camera> camerasToDisable, Camera cameraToEnable, MonoBehaviour playerControlScript)
    {
        // Fade Out
        yield return StartCoroutine(Fade(1));

        // Desactiva controles y cámaras del jugador
        if (playerControlScript != null)
            playerControlScript.enabled = false;

        foreach (Camera cam in camerasToDisable)
        {
            if (cam != null)
                cam.enabled = false;
        }

        if (cameraToEnable != null)
            cameraToEnable.enabled = true;

        // Teletransporta
        player.position = destination;

        // Fade In
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}
