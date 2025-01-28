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
    [SerializeField] private float hamsterDensity = 0.5f;
    [SerializeField] private MeshRenderer hamsterbubble;

    [SerializeField] private float recoveryBubbles = 0;

    private PlayerPoints uiPoints;
    [SerializeField] private GameObject uiPointsPrefab;

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
    [SerializeField]private List <BoostBubble> myBubbles;

    private float initialDensity;
    private bool hasBubble;
    private bool isGround;

    public bool ImDie;

    void Start()
    {
        uiPoints = Instantiate(uiPointsPrefab).GetComponent<PlayerPoints>();
        GameManager.Instance.SetNewPlayer(uiPoints);
        ImDie = false;
        myBubbles = new List<BoostBubble>();
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
        initialDensity = rb.mass;
        hasBubble = true;
    }

    public PlayerPoints GetPlayerPointsUI()
    {
        return uiPoints;
    }

    private void OnDestroy()
    {
        // Desuscribirse de los eventos
        attackAction.started -= OnAttackStarted;
        attackAction.canceled -= OnAttackCanceled;
    }

    public void ExploteBubble()
    {
        if (hasBubble)
        {
            rb.mass = hamsterDensity;
            hasBubble = false;
            hamsterbubble.enabled = false;
            smoothTime = 3;
            recoveryBubbles = 0;
            //rb.constraints = RigidbodyConstraints.FreezePositionY;
            foreach (var item in myBubbles)
            {
                item.gameObject.SetActive(true);
                item.transform.position = gameObject.transform.position;
                item.DropBubble();

            }
            myBubbles.Clear();
            bubbles = myBubbles.Count;
        }
    }

    public void ReturBubble()
    {
        rb.mass = initialDensity;
        hasBubble = true;
        hamsterbubble.enabled = true;
        smoothTime = originalSmoothTime;
        recoveryBubbles = 0;
        //rb.constraints = RigidbodyConstraints.None;


    }

    private void OnCollisionStay(Collision collision)
    {

        if (hasBubble == false&& isGround==false)
        {
            if (collision.gameObject.CompareTag("ground"))
            {
                smoothTime = 0;

                isGround = true;
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (hasBubble == false)
        {
            if (collision.gameObject.CompareTag("ground"))
            {
                smoothTime = initialSmoothTime;
                isGround = false;
            }
        }
    }



    void Update()
    {
        if (ImDie)
            return;
        // Leer entrada de movimiento
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(inputDir.x, 0f, inputDir.y).normalized;

        if (hasBubble)
        {
            if (xlr8)
            {

                if (bubbles > 0)
                {
                    moveSpeed = moveSpeedBoost;
                    bubbles -= 1 * Time.deltaTime;
                    if (bubbles < myBubbles.Count)
                    {
                        if ((myBubbles.Count - 1) >= 0)
                        {
                            myBubbles[myBubbles.Count - 1].gameObject.SetActive(true);
                            myBubbles[myBubbles.Count - 1].transform.position = gameObject.transform.position;
                            myBubbles[myBubbles.Count - 1].DropBubble();
                            myBubbles.Remove(myBubbles[myBubbles.Count - 1]);
                        }
                    }

                }
                else if (bubbles <= 0)
                {
                    bubbles = 0;
                    moveSpeed = initialSpeed;
                    //xlr8 = false;

                }
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
        if (hasBubble)
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
        else
        {
            if (isGround)
            {
                rb.drag = Mathf.MoveTowards(rb.drag, originalDrag, dragRecoverySpeed * Time.deltaTime);
                smoothTime = Mathf.MoveTowards(smoothTime, 0, smoothTimeRecoverySpeed * Time.deltaTime);

                Vector3 playerForce = moveDirection * moveSpeed;
                rb.AddForce(playerForce, ForceMode.Force);

                if (Mathf.Approximately(rb.drag, originalDrag) && Mathf.Approximately(smoothTime, 0))
                {
                    launched = false;
                }
            }
            else
            {
                rb.drag = Mathf.MoveTowards(rb.drag, originalDrag, dragRecoverySpeed * Time.deltaTime);
                smoothTime = Mathf.MoveTowards(smoothTime, 1f, smoothTimeRecoverySpeed * Time.deltaTime);

                Vector3 playerForce = moveDirection * moveSpeed;
                rb.AddForce(playerForce, ForceMode.Force);

                if (Mathf.Approximately(rb.drag, originalDrag) && Mathf.Approximately(smoothTime, 1f))
                {
                    launched = false;
                }
            }
        }
    }

    public void DieEvent()
    {
        ImDie = true;
        gameObject.SetActive(false);
        
    }
    public void ResetLive()
    {
        rb.mass = initialDensity;
        hasBubble = true;
        hamsterbubble.enabled = true;
        smoothTime = originalSmoothTime;
        recoveryBubbles = 0;
        //rb.constraints = RigidbodyConstraints.None;
        myBubbles.Clear();
        bubbles = myBubbles.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        BoostBubble bubble = other.gameObject.GetComponent<BoostBubble>();
        
            if (bubbles < maxBubbles)
            {
                if (bubble != null && bubble.GetLastPlayer() != this)
                {
                    if (hasBubble)
                    {
                        bubble.SetLastPlayer(this);
                        myBubbles.Add(bubble);
                        bubble.gameObject.SetActive(false);
                        bubbles = myBubbles.Count;
                    }
                    else
                    {
                        recoveryBubbles += 1;
                        bubble.gameObject.SetActive(false);
                        if (recoveryBubbles >= 3)
                        {
                            ReturBubble();
                        }
                    }
                }
            }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Water"))
        {
            if (!hasBubble)
            {
                DieEvent();
            }
        }

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

        ICollisionProp prop = collision.gameObject.GetComponent<ICollisionProp>();

        if(prop!=null)
        {
           if(prop is DuckProp)
           {
                Vector3 relativeVelocity = rb.velocity;
                Vector3 impactForce = (relativeVelocity * launchForceMultiplier)* 1.2f;
                
                //ExploteBubble();
                
                // Aplicar lanzamiento a ambos jugadores
                ApplyLaunch(impactForce);
           }
        }

        if (collision.gameObject.CompareTag("pipe"))
        {
            ExploteBubble();
        }




    }
}
