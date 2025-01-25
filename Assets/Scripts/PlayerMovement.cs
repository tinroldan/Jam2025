using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;  // Velocidad de movimiento
    public float smoothTime = 0.1f;  // Tiempo de suavizado para detenerse rápidamente
    public float maxSpeed = 10f;   // Velocidad máxima que la bola puede alcanzar
    public float launchForce = 20f; // Fuerza de lanzamiento al soltar la tecla L
    public float dragRecoverySpeed = 1f; // Velocidad a la que se recupera el drag
    public float smoothTimeRecoverySpeed = 0.5f; // Velocidad a la que se recupera el smoothTime
    public float launchCooldown = 2f; // Tiempo de espera para volver a lanzar

    private Rigidbody rb;          // Componente Rigidbody
    private Vector3 currentVelocity;  // Para el SmoothDamp
    private Vector3 launchDirection; // Dirección hacia donde se lanzará
    private bool isAiming = false;   // Indica si estamos apuntando (tecla L presionada)
    private bool launched = false;  // Indica si la bola fue lanzada
    private bool canLaunch = true;  // Indica si es posible lanzar
    private float originalSmoothTime; // Almacena el smoothTime original

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Configuramos el Rigidbody para que tenga poco arrastre y rotación
        rb.drag = 5f;  // Drag (resistencia al movimiento en el eje X y Z)
        rb.angularDrag = 5f;  // Resistencia al movimiento rotacional

        originalSmoothTime = smoothTime; // Guardamos el valor inicial de smoothTime
    }

    void Update()
    {
        // Obtenemos el input de las teclas de movimiento (WASD o las flechas)
        float moveX = Input.GetAxis("Horizontal");  // Teclas A/D o flechas izquierda/derecha
        float moveZ = Input.GetAxis("Vertical");    // Teclas W/S o flechas arriba/abajo

        // Calculamos la dirección del movimiento
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (Input.GetKey(KeyCode.L) && canLaunch) // Si se mantiene presionada la tecla L y se puede lanzar
        {
            if (!isAiming)
            {
                // Al comenzar a apuntar, reducimos la velocidad progresivamente
                isAiming = true;
                launched = false;
                smoothTime = 1f; // Incrementamos el smoothTime para apuntar
            }

            // Aplicamos una lógica de desaceleración progresiva al apuntar
            Vector3 targetVelocity = Vector3.zero;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);

            // Guardamos la dirección hacia donde estamos apuntando
            if (moveDirection.magnitude > 0)
            {
                launchDirection = moveDirection;
            }
        }
        else if (Input.GetKeyUp(KeyCode.L) && canLaunch) // Al soltar la tecla L y se puede lanzar
        {
            // Lanzamos la bola en la dirección almacenada
            rb.drag = 0f; // Drag inicial para movimiento libre
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
            isAiming = false;
            launched = true;
            canLaunch = false; // Iniciamos el cooldown
            StartCoroutine(LaunchCooldown());
        }

        if (launched) // Recuperar drag y smoothTime progresivamente mientras permite movimiento
        {
            rb.drag = Mathf.MoveTowards(rb.drag, 5f, dragRecoverySpeed * Time.deltaTime);
            smoothTime = Mathf.MoveTowards(smoothTime, originalSmoothTime, smoothTimeRecoverySpeed * Time.deltaTime);

            if (moveDirection.magnitude > 0)
            {
                Vector3 targetVelocity = moveDirection * moveSpeed;
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            }
        }
        else if (!isAiming) // Movimiento normal si no estamos apuntando ni lanzados
        {
            if (moveDirection.magnitude > 0)
            {
                // Aceleración rápida a la velocidad deseada
                Vector3 targetVelocity = moveDirection * moveSpeed;
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            }
            else
            {
                // Si no hay input, desacelerar rápidamente
                Vector3 targetVelocity = Vector3.zero;
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            }
        }
    }

    IEnumerator LaunchCooldown()
    {
        yield return new WaitForSeconds(launchCooldown);
        canLaunch = true;
    }
}
