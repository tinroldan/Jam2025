using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterRotationConstrain : MonoBehaviour
{
    private Quaternion lastParentRotation;
    void Start()
    {
        lastParentRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) * lastParentRotation * transform.localRotation;

        lastParentRotation = transform.parent.localRotation;
    }
}
