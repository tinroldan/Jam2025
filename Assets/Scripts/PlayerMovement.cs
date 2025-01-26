using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Velocidad de movimiento
    [SerializeField] private float smoothTime = 0.1f; // Tiempo de suavizado para detenerse
    [SerializeField] private float launchForce = 20f; // Fuerza del lanzamiento
    [SerializeField] private float dragRecoverySpeed = 1f; // Velocidad de recuperación del drag
    [SerializeField] private float smoothTimeRecoverySpeed = 0.5f; // Velocidad de recuperación del smoothTime
    [SerializeField] private float launchCooldown = 2f; // Tiempo de espera para lanzar de nuevo
    [SerializeField] private GameObject pivot; // Pivot con la flecha

    private Rigidbody rb;
    private Vector3 currentVelocity; // Velocidad actual para SmoothDamp
    private Vector3 launchDirection; // Dirección de lanzamiento
    private bool isAiming = false;
    private bool launched = false;
    private bool canLaunch = true;
    private bool recovering = false; // Si está recuperando el control
    private float originalSmoothTime; // Valor original de smoothTime

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 5f;
        rb.angularDrag = 5f;
        originalSmoothTime = smoothTime;

        // Configurar el New Input System
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];

        // Suscribirse a los eventos del botón de ataque
        attackAction.started += OnAttackStarted;
        attackAction.canceled += OnAttackCanceled;
    }

    private void OnDestroy()
    {
        // Desuscribirse de los eventos
        attackAction.started -= OnAttackStarted;
        attackAction.canceled -= OnAttackCanceled;
    }

    void Update()
    {
        if (recovering) return; // No permitir movimiento normal mientras se recupera

        // Leer entrada de movimiento
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDir.x, 0f, inputDir.y).normalized;

        // Apuntar el pivot en la dirección de movimiento
        if (pivot && moveDirection.magnitude > 0)
        {
            pivot.transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        if (!isAiming && !launched)
        {
            // Movimiento normal
            Vector3 targetVelocity = moveDirection * moveSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }
        else if (isAiming)
        {
            // Frenado progresivo al apuntar
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, smoothTime);
        }

        // Recuperación progresiva tras el lanzamiento
        if (launched)
        {
            rb.drag = Mathf.MoveTowards(rb.drag, 5f, dragRecoverySpeed * Time.deltaTime);
            smoothTime = Mathf.MoveTowards(smoothTime, originalSmoothTime, smoothTimeRecoverySpeed * Time.deltaTime);
        }
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (canLaunch)
        {
            // Iniciar apuntado
            isAiming = true;
            launched = false;
            smoothTime = 1f; // Frenado suave
        }
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (isAiming && canLaunch)
        {
            // Configurar dirección de lanzamiento
            if (moveDirection.magnitude > 0)
            {
                launchDirection = moveDirection;
            }
            else
            {
                launchDirection = pivot.transform.forward; // Dirección del pivot si no hay movimiento
            }

            // Aplicar el lanzamiento
            ApplyLaunch(launchDirection * launchForce);
            isAiming = false;
            canLaunch = false;

            // Iniciar el cooldown
            StartCoroutine(LaunchCooldown());
        }
    }

    private void ApplyLaunch(Vector3 force)
    {
        rb.drag = 0f; // Reducir el drag para permitir el movimiento
        rb.velocity = Vector3.zero; // Asegurarse de que no haya acumulación de velocidad previa
        rb.AddForce(force, ForceMode.Impulse); // Aplicar la fuerza de lanzamiento
        launched = true;
        StartCoroutine(RecoverControl());
    }

    private IEnumerator RecoverControl()
    {
        yield return new WaitForSeconds(2f); // Tiempo de recuperación
        recovering = false;
        launched = false;
    }

    private IEnumerator LaunchCooldown()
    {
        yield return new WaitForSeconds(launchCooldown);
        canLaunch = true;
    }
}
