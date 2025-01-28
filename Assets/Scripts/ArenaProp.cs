using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaProp : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bounds"))
        {
            transform.position = startPos;
        }
    }
}
