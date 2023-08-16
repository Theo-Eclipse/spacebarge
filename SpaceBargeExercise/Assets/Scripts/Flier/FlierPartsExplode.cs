using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlierPartsExplode : MonoBehaviour
{
    public float partsRotationSpeed = 300.0f;
    public float partsLinearSpeed = 5;
    // Start is called before the first frame update
    [SerializeField] private Transform targetGraphics;
    private List<Collider> shipParts = new List<Collider>();
    private Rigidbody partRigidbody;
    private MeshRenderer partRenderer;
    private Dictionary<Collider, FlierWreckage> parts = new Dictionary<Collider, FlierWreckage>();
    void OnEnable()
    {
        ExplodeWreks();
    }

    private void ExplodeWreks() 
    {
        targetGraphics.GetComponentsInChildren(true, shipParts);
        foreach (var collider in shipParts)
        {
            partRenderer = collider.GetComponent<MeshRenderer>();
            partRigidbody = collider.GetComponent<Rigidbody>();
            if (partRenderer && !partRigidbody)
            {
                partRigidbody = CreateRigidBody(collider);
            }
            else if (parts.ContainsKey(collider)) 
            {
                ResetPart(collider);
                partRigidbody = parts[collider].rigidbody;
            }
            ApplyForceAndTorque(partRigidbody);
        }
    }
    private Rigidbody CreateRigidBody(Collider collider) 
    {
        partRigidbody = collider.gameObject.AddComponent<Rigidbody>();
        partRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        partRigidbody.useGravity = false;
        parts.Add(collider, new FlierWreckage
        {
            rigidbody = partRigidbody,
            initialPosition = collider.transform.position,
            initialRotation = collider.transform.rotation
        });
        return partRigidbody;
    }
    private void ApplyForceAndTorque(Rigidbody part) 
    {
        if (!part)
            return;
        part.AddTorque(new Vector3((Random.value - 0.5f) * partsRotationSpeed, (Random.value - 0.5f) * partsRotationSpeed, (Random.value - 0.5f) * partsRotationSpeed) * 2, ForceMode.Impulse);
        part.AddForce((targetGraphics.position - part.position).normalized * Random.value * partsLinearSpeed, ForceMode.Impulse);
    }

    private void ResetPart(Collider collider)
    {
        collider.transform.position = parts[collider].initialPosition;
        collider.transform.rotation = parts[collider].initialRotation;
    }

    private struct FlierWreckage 
    {
        public Rigidbody rigidbody;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
    }
}
