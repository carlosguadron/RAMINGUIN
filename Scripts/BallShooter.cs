using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [Header("Referencias")]
    public Transform ball;
    public Transform arrowPivot;
    public Transform arrow;

    [Header("Configuración")]
    public float rotationSpeed = 90f;
    public float fuerzaMinima = 10f;
    public float fuerzaMaxima = 20f;

    private Rigidbody rb;
    private bool isLaunched = false;
    private bool combateActivo = false;
    private bool esperandoTurno = false; // Nueva variable

    private void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
        PrepararParaDisparo();
    }

    private void Update()
    {
        if (TurnUI.Instance != null && TurnUI.Instance.IsPanelActive())
        {
            esperandoTurno = true;
            return;
        }

        // Solo restablecer si estábamos esperando y ahora no
        if (esperandoTurno)
        {
            esperandoTurno = false;
            if (!isLaunched && combateActivo)
            {
                PrepararParaDisparo();
            }
        }

        if (!combateActivo || isLaunched) return;

        arrowPivot.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        Vector3 direction = (arrow.position - ball.position).normalized;
        float fuerzaFinal = Random.Range(fuerzaMinima, fuerzaMaxima);

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(direction * fuerzaFinal, ForceMode.Impulse);
        isLaunched = true;
        arrow.gameObject.SetActive(false);
    }
    public void ResetearTodo()
    {
        combateActivo = false;
        isLaunched = false;

        if (arrow != null)
            arrow.gameObject.SetActive(false);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    public void ActivarCombate()
    {
        combateActivo = true;
        isLaunched = false;
        PrepararParaDisparo();
    }

    private void PrepararParaDisparo()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        arrowPivot.position = ball.position;
        arrowPivot.rotation = Quaternion.identity;
        arrow.gameObject.SetActive(true);
    }

    public void ResetearBola()
    {
        isLaunched = false;
        PrepararParaDisparo();
    }
}