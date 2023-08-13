using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FlierStats
{
    [Space, Header("Flier Settings")]
    public float maxVelocity;
    public float maxAngularVelocity;
    public float forwardSpeed;
    public float maneurSpeed;
    public float rotationSpeed;
    public float velocityDumping;
}
