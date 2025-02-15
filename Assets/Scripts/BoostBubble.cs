using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoostBubble : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement lastPlayer;
    private SphereCollider collider;

    private Rigidbody rb;

    public float forceMagnitude = 5f;
    public bool isOnAir = false;
    public float initialHeight;

    public PlayerMovement GetLastPlayer()
    {
        return lastPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;

    }
    private void OnEnable()
    {
        
    }

    public void DropBubble()
    {
        StartCoroutine(PickCooldown());
        ApplyRandomHorizontalForce();
    }

    public void SetLastPlayer(PlayerMovement player)
    {
        lastPlayer = player;
    }
    private void Update()
    {
        if(isOnAir)
        {
            ReturnToLevel();
        }
    }

    public void ApplyRandomHorizontalForce()
    {
        // Generar una dirección random solo en el plano XZ
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        // Calcular la fuerza y aplicarla al Rigidbody
        Vector3 force = randomDirection * forceMagnitude;
        rb.AddForce(force, ForceMode.Impulse); // Usar fuerza en modo "impulso"
    }

    private IEnumerator PickCooldown()
    {
        yield return new WaitForSeconds(1f);
        lastPlayer = null;
        collider.enabled = false;
        collider.enabled = true;
    }

    private void ReturnToLevel()
    {
        float currentHeight = transform.position.y;
        transform.position = new Vector3( transform.position.x, currentHeight -= (0.5f * Time.deltaTime), transform.position.z);
        if(transform.position.y <= initialHeight)
        {
            isOnAir = false;
        }
        //transform.localPosition = new Vector3(transform.localPosition.x, currentWaterHeight += (floodingSpeed * Time.deltaTime), transform.localPosition.z);
    }

            

    IEnumerator ResetPosition()
    {
        yield break;
    }
}
