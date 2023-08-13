using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicFlier : MonoBehaviour, IDirectionInputHandler
{
    [Header("Flier Components")]
    [SerializeField] protected Rigidbody flierBody;

    [Header("Flier Thrusters")]
    [SerializeField] private ParticleSystem[] forwardThrusters;

    [Space, Header("Flier Stats")]
    public FlierStats stats = new FlierStats() {
        maxVelocity = 10.0f,
        maxAngularVelocity = 200,
        forwardSpeed = 8,
        maneurSpeed = 3,
        rotationSpeed = 500,
        velocityDumping = 0.98f
    };

    [Header("Flier Inputs")]
    [Range(0, 1)]
    public float thrustPower = 0.0f;
    public Vector3 thrustInputDirection;
    public Vector3 headingDirection;
    public Transform lockAtTarget;

    protected float AngleOfDirection => Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(headingDirection.x, headingDirection.z));
    protected float AngleToAxisInput => Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(thrustInputDirection.x, thrustInputDirection.z));
    private float TorqueToDirection => (Mathf.InverseLerp(90, -90, AngleOfDirection) - 0.5f) * 2.0f;
    private bool HasThrustInput => thrustPower > 0.01f && thrustInputDirection.magnitude > 0.01f;
    private bool HasTargetOrMoving => lockAtTarget || HasThrustInput;
    private bool CanThrustForward => (Mathf.Abs(AngleToAxisInput) < 5 || thrustPower > 0.5f) && Mathf.Abs(AngleToAxisInput) < 120;

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
        if(HasTargetOrMoving)
            RotateTowardsTarget();
        if (HasThrustInput)
            ThrustManeur();
        if (HasThrustInput && CanThrustForward)
            ThrustForward();

        foreach (var thruster in forwardThrusters)
            if (CanThrustForward && thrustPower >= 0.25f && !thruster.isPlaying)
                thruster.Play();
            else if ((!CanThrustForward || thrustPower < 0.25f) && thruster.isPlaying)
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
        if (Mathf.Abs(flierBody.angularVelocity.y) < stats.maxAngularVelocity * Mathf.Deg2Rad)
            flierBody.AddTorque(new Vector3(0, stats.rotationSpeed * Mathf.Deg2Rad * TorqueToDirection, 0), ForceMode.Force);
        flierBody.angularVelocity = new Vector3(0, Mathf.Clamp(flierBody.angularVelocity.y, -stats.maxAngularVelocity * Mathf.Deg2Rad, stats.maxAngularVelocity * Mathf.Deg2Rad), 0);
    }

    private void ThrustForward()
    {
        flierBody.AddForce(transform.forward * stats.forwardSpeed * thrustPower, ForceMode.Force);
        ClampVelocity();
    }
    private void ThrustManeur()
    {
        flierBody.AddForce(thrustInputDirection * stats.maneurSpeed * thrustPower, ForceMode.Force);
        ClampVelocity();
    }

    private void ClampVelocity() 
    {
        if(flierBody.velocity.magnitude > stats.maxVelocity)
            flierBody.velocity = flierBody.velocity.normalized * stats.maxVelocity;
    }
    private void ApplyVelocityAndInputDumping()
    {
        thrustPower = Mathf.Clamp01(thrustPower - Time.deltaTime * 2);
        if (thrustPower <= 0.5f)
            flierBody.velocity *= stats.velocityDumping;
    }

    public void HandleInput(Vector2 input)
    {
        thrustInputDirection = new Vector3(input.x, 0, input.y);
    }
}
