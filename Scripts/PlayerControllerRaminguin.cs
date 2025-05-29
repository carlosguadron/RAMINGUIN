using UnityEngine;

public class PlayerControllerRaminguin : MonoBehaviour
{
    [Header("Configuración Básica")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    [Header("Referencias de Cámaras")]
    public Transform playerCamera;      // Cámara en 1ra persona
    public Transform thirdPersonCam;    // Cámara en 3ra persona
    public Animator animator;

    [Header("Estado del Control")]
    [SerializeField] private bool controlActivo = true;
    [SerializeField] private bool isFirstPerson = true;

    private CharacterController characterController;
    private float rotationX = 0;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        SetControlActive(true); // Estado inicial con controles activos
        UpdateCameraState();
    }

    void Update()
    {
        if (!controlActivo) return;

        HandleMovement();
        HandleCameraRotation();
        HandleCameraSwitch();
    }

    #region Métodos de Control
    public void SetControlActive(bool activo)
    {
        controlActivo = activo;

        // Manejo del cursor
        Cursor.visible = !activo;
        Cursor.lockState = activo ? CursorLockMode.Locked : CursorLockMode.None;

        // Actualizar estado de animación
        if (animator != null)
        {
            animator.SetBool("CaminarRaminguin", false);
        }
    }

    public void RestaurarCamaras()
    {
        UpdateCameraState();
    }
    #endregion

    #region Lógica de Movimiento
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("CaminarRaminguin", move.magnitude > 0);
        }
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            UpdateCameraState();
        }
    }

    private void UpdateCameraState()
    {
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(isFirstPerson && controlActivo);
        if (thirdPersonCam != null)
            thirdPersonCam.gameObject.SetActive(!isFirstPerson && controlActivo);
    }
    #endregion

    #region Métodos Públicos para Combate
    public void ActivarModoCombate()
    {
        SetControlActive(false);

        // Asegurarse que ambas cámaras del jugador están desactivadas
        if (playerCamera != null) playerCamera.gameObject.SetActive(false);
        if (thirdPersonCam != null) thirdPersonCam.gameObject.SetActive(false);
    }

    public void DesactivarModoCombate()
    {
        SetControlActive(true);
        RestaurarCamaras();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ResetearCompletamente()
    {
        SetControlActive(true);
        RestaurarCamaras();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reactivar ambas cámaras por si acaso
        if (playerCamera != null) playerCamera.gameObject.SetActive(true);
        if (thirdPersonCam != null) thirdPersonCam.gameObject.SetActive(true);
    }


    #endregion
}