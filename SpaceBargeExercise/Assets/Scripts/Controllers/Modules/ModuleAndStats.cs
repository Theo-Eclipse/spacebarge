using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.ModulesAndStats
{
    [System.Serializable]
    public struct FlierStats
    {
        public float maxVelocity;
        public float maxAngularVelocity;
        public float forwardSpeed;
        public float maneurSpeed;
        public float rotationSpeed;
        public float velocityDumping;
    }
}
