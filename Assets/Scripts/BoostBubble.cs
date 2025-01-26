using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBubble : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement lastPlayer;
    private SphereCollider collider;
    public PlayerMovement GetLastPlayer()
    {
        return lastPlayer;
    }

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
    }
    private void OnEnable()
    {
        StartCoroutine(PickCooldown());
    }

    public void SetLastPlayer(PlayerMovement player)
    {
        lastPlayer = player;
    }

    private IEnumerator PickCooldown()
    {
        yield return new WaitForSeconds(1f);
        lastPlayer = null;
        collider.enabled = false;
        collider.enabled = true;
    }
}
