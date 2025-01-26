using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBubble : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement lastPlayer;
    private SphereCollider collider;

    private Rigidbody rb;

    public float forceMagnitude = 5f;
    public PlayerMovement GetLastPlayer()
    {
        return lastPlayer;
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
}
