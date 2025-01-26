using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckProp : MonoBehaviour, ICollisionProp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ICollisionProp.OnPlayerCollision(float bounceRate)
    {
        Debug.Log("El que lo lea es gay");
    }
}
