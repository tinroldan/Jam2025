using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Velocidad de movimiento
    [SerializeField] private float moveSpeedBoost = 20f;
    [SerializeField] private float smoothTime = 0.1f; // Tiempo de suavizado para detenerse
    [SerializeField] private float launchForceMultiplier = 1f; // Multiplicador para fuerza de lanzamiento
    [SerializeField] private float initialSmoothTime = 1f; // SmoothTime inicial al lanzar
    [SerializeField] private float dragRecoverySpeed = 1f; // Velocidad de recuperación del drag
    [SerializeField] private float smoothTimeRecoverySpeed = 0.5f; // Velocidad de recuperación del smoothTime
    [SerializeField] private GameObject pivot; // Pivot con la flecha
    [SerializeField] private GameObject hamster; // Pivot con la flecha
    [SerializeField] private Animator anim; // Pivot con la flecha
    [SerializeField] private float bubbles=0;
    [SerializeField] private float maxBubbles = 3;

    private Rigidbody rb;
    private Vector3 currentVelocity; // Velocidad actual para SmoothDamp
    private bool launched = false; // Si está en estado lanzado
    private float originalSmoothTime; // Valor original de smoothTime
    private float originalDrag; // Valor original del drag

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;
    private Vector3 moveDirection;
    private float initialSpeed;
    private bool xlr8;
    private List <GameObject> myBubbles;

    void Start()
    {
        myBubbles = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        rb.angularDrag = 5f;
        originalSmoothTime = smoothTime;
        initialSpeed = moveSpeed;

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
        // Leer entrada de movimiento
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDir.x, 0f, inputDir.y).normalized;

        if(xlr8)
        {
            
            if(bubbles>0)
            {
                moveSpeed = moveSpeedBoost;
                bubbles -= 1 * Time.deltaTime;
                if(bubbles<myBubbles.Count)
                {
                    if ((myBubbles.Count - 1) >= 0)
                    {
                        myBubbles[myBubbles.Count - 1].gameObject.SetActive(true);
                        myBubbles[myBubbles.Count - 1].transform.position = gameObject.transform.position;
                        myBubbles.Remove(myBubbles[myBubbles.Count - 1]);
                    }
                }

            }
            else if(bubbles <= 0)
            {
                bubbles = 0;
                moveSpeed = initialSpeed;
                //xlr8 = false;

            }
        }


        if (pivot && moveDirection.magnitude > 0)
        {
            pivot.transform.rotation = Quaternion.LookRotation(moveDirection);
            hamster.transform.rotation = Quaternion.LookRotation(moveDirection);
            anim.SetBool("isRuning", true);
        }
        else
        {
            anim.SetBool("isRuning", false);
        }

        if (launched)
        {
            HandleRecoveryAndMovement();
        }
        else
        {
            // Movimiento normal cuando no está lanzado
            Vector3 targetVelocity = moveDirection * moveSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        // Iniciar lógica de ataque si es necesario
        
            xlr8 = true;
        
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        // Lógica de ataque finalizada si es necesario
        moveSpeed = initialSpeed;
        xlr8 = false;
    }

    private void ApplyLaunch(Vector3 force)
    {
        rb.drag = 0f; // Reducir el drag para permitir movimiento libre
        rb.velocity = Vector3.zero; // Reiniciar la velocidad previa
        rb.AddForce(force, ForceMode.Impulse); // Aplicar la fuerza de lanzamiento
        launched = true;
        smoothTime = initialSmoothTime; // Inicializar el smoothTime en su valor inicial alto
    }

    private void HandleRecoveryAndMovement()
    {
        // Recuperación progresiva
        rb.drag = Mathf.MoveTowards(rb.drag, originalDrag, dragRecoverySpeed * Time.deltaTime);
        smoothTime = Mathf.MoveTowards(smoothTime, originalSmoothTime, smoothTimeRecoverySpeed * Time.deltaTime);

        // Movimiento del jugador durante la recuperación
        Vector3 playerForce = moveDirection * moveSpeed;
        rb.AddForce(playerForce, ForceMode.Force);

        // Finalizar recuperación si los valores han vuelto a la normalidad
        if (Mathf.Approximately(rb.drag, originalDrag) && Mathf.Approximately(smoothTime, originalSmoothTime))
        {
            launched = false; // Salir del estado lanzado
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bubbles < maxBubbles)
        {
            BoostBubble bubble = other.gameObject.GetComponent<BoostBubble>();
            if (bubble != null && bubble.GetLastPlayer() != this)
            {
                bubble.SetLastPlayer(this);
                myBubbles.Add(other.gameObject);
                bubble.gameObject.SetActive(false);
                bubbles = myBubbles.Count;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement otherPlayer = collision.gameObject.GetComponent<PlayerMovement>();
        if (otherPlayer != null)
        {
            // Combinar velocidades para calcular las fuerzas de lanzamiento
            Vector3 relativeVelocity = rb.velocity - otherPlayer.rb.velocity;
            Vector3 impactForce = relativeVelocity * launchForceMultiplier;

            // Aplicar lanzamiento a ambos jugadores
            ApplyLaunch(impactForce);
            otherPlayer.ApplyLaunch(-impactForce);
        }
    }
}
