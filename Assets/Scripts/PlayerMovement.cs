using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Velocidad de movimiento
    [SerializeField] private float smoothTime = 0.1f; // Tiempo de suavizado para detenerse
    [SerializeField] private float maxSpeed = 10f; // Velocidad máxima
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 5f;
        rb.angularDrag = 5f;
        originalSmoothTime = smoothTime;
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    void Update()
    {
        if (recovering) return; // No permitir movimiento normal mientras se recupera

        //float moveX = Input.GetAxis("Horizontal");
        //float moveZ = Input.GetAxis("Vertical");
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputDir.x, 0f, inputDir.y).normalized;

        if (pivot && moveDirection.magnitude > 0)
        {
            pivot.transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        if (Input.GetButton("Fire1") && canLaunch)
        {
            if (!isAiming)
            {
                isAiming = true;
                launched = false;
                smoothTime = 1f;
            }

            if (moveDirection.magnitude > 0)
            {
                launchDirection = moveDirection;
            }
        }
        else if (Input.GetButtonUp("Fire1") && canLaunch)
        {
            ApplyLaunch(launchDirection * launchForce);
            isAiming = false;
            canLaunch = false;
            StartCoroutine(LaunchCooldown());
        }

        if (launched)
        {
            rb.drag = Mathf.MoveTowards(rb.drag, 5f, dragRecoverySpeed * Time.deltaTime);
            smoothTime = Mathf.MoveTowards(smoothTime, originalSmoothTime, smoothTimeRecoverySpeed * Time.deltaTime);
        }
        else
        {
            Vector3 targetVelocity = moveDirection * moveSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement otherPlayer = collision.gameObject.GetComponent<PlayerMovement>();
        if (otherPlayer != null && launched)
        {
            // Transmitir la inercia del impacto
            Vector3 impactForce = rb.velocity + otherPlayer.rb.velocity; // Combinar inercia
            otherPlayer.ApplyLaunch(impactForce);
        }
    }

    public void ApplyLaunch(Vector3 force)
    {
        rb.drag = 0f; // Resetear el drag para permitir movimiento libre
        rb.velocity = Vector3.zero; // Asegurarnos de que no se acumule velocidad previa
        rb.AddForce(force, ForceMode.Impulse); // Aplicar la fuerza
        launched = true;
        recovering = true; // Iniciar recuperación
        StartCoroutine(RecoverControl());
    }

    IEnumerator RecoverControl()
    {
        yield return new WaitForSeconds(1f); // Tiempo de recuperación
        recovering = false;
    }

    IEnumerator LaunchCooldown()
    {
        yield return new WaitForSeconds(launchCooldown);
        canLaunch = true;
    }
}
