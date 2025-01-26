using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionProp
{
    //public float BounceRate { get; set; }
    void OnPlayerCollision(float bounceRate)
    {

    }
}
