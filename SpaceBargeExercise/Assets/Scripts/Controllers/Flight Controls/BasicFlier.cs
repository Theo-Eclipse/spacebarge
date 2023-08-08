using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicFlier : MonoBehaviour, IDirectionInputHandler
{
    [Header("Flier Components")]
    [SerializeField] private Rigidbody flierBody;

    [Header("Flier Thrusters")]
    [SerializeField] private ParticleSystem[] forwardThrusters;

    [Header("Flier Settings")]
    public float maxVelocity = 10;
    public float maxAngularVelocity = 200;
    public float forwardSpeed = 8;
    public float maneurSpeed = 3;
    public float rotationSpeed = 800;
    public float velocityDumping = 0.99f;

    [Header("Flier Inputs")]
    [Range(0, 1)]
    public float thrustPower = 0.0f;
    public Vector3 thrustInputDirection;
    public Vector3 headingDirection;
    public Transform lockAtTarget;

    private float angleToDirection => Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(headingDirection.x, headingDirection.z));
    private float angleToAxisInput => Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(thrustInputDirection.x, thrustInputDirection.z));
    private float torqueToDirection => (Mathf.InverseLerp(90, -90, angleToDirection) - 0.5f) * 2.0f;
    private bool hasThrustInput => thrustPower > 0.01f && thrustInputDirection.magnitude > 0.01f;
    private bool hasTargetOrMoving => lockAtTarget || hasThrustInput;
    private bool canThrustForward => (Mathf.Abs(angleToAxisInput) < 5 || thrustPower > 0.5f) && Mathf.Abs(angleToAxisInput) < 120;

    // Reset is called once the this component added to a game object.
    private void Reset()
    {
        flierBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        headingDirection = transform.forward;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetHeading();
        if(hasTargetOrMoving)
            RotateTowardsTarget();
        if (hasThrustInput)
            ThrustManeur();
        if (hasThrustInput && canThrustForward)
            ThrustForward();

        foreach (var thruster in forwardThrusters)
            if (canThrustForward && thrustPower >= 0.25f && !thruster.isPlaying)
                thruster.Play();
            else if ((!canThrustForward || thrustPower < 0.25f) && thruster.isPlaying)
                thruster.Stop();

        ApplyVelocityAndInputDumping();
    }

    private void GetHeading() 
    {
        if (lockAtTarget)
            headingDirection = lockAtTarget.position - transform.position;
        else headingDirection = thrustInputDirection.normalized;
    }

    private void RotateTowardsTarget() 
    {
        if (Mathf.Abs(flierBody.angularVelocity.y) < maxAngularVelocity * Mathf.Deg2Rad)
            flierBody.AddTorque(new Vector3(0, rotationSpeed * Mathf.Deg2Rad * torqueToDirection, 0), ForceMode.Force);
        flierBody.angularVelocity = new Vector3(0, Mathf.Clamp(flierBody.angularVelocity.y, -maxAngularVelocity * Mathf.Deg2Rad, maxAngularVelocity * Mathf.Deg2Rad), 0);
    }

    private void ThrustForward()
    {
        flierBody.AddForce(transform.forward * forwardSpeed * thrustPower, ForceMode.Force);
        ClampVelocity();
    }
    private void ThrustManeur()
    {
        flierBody.AddForce(thrustInputDirection * maneurSpeed * thrustPower, ForceMode.Force);
        ClampVelocity();
    }

    private void ClampVelocity() 
    {
        if(flierBody.velocity.magnitude > maxVelocity)
            flierBody.velocity = flierBody.velocity.normalized * maxVelocity;
    }
    private void ApplyVelocityAndInputDumping()
    {
        thrustPower = Mathf.Clamp01(thrustPower - Time.deltaTime * 2);
        if (thrustPower <= 0.5f)
            flierBody.velocity *= velocityDumping;
    }

    public void HandleInput(Vector2 input)
    {
        thrustInputDirection = new Vector3(input.x, 0, input.y);
    }
}