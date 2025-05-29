using UnityEngine;

public class Pin : MonoBehaviour
{
    public GameObject destroyEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (PinManager.Instance != null)
            {
                PinManager.Instance.PinDerribado();
            }

            if (destroyEffect != null)
                Instantiate(destroyEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}