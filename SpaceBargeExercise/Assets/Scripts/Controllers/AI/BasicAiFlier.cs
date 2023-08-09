using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAiFlier : BasicFlier
{
    [Header("AI Waypoint")]
    public Vector3 waypointPosition = Vector3.zero;
    public float maxThrustDistance = 12.5f;
    public float reachPointDistance = 0.5f;
    public bool move = false;

    private Vector2 InputAxis => new Vector2(waypointPosition.x - transform.position.x, waypointPosition.z - transform.position.z);

    // Update is called once per frame
    protected override void Update()
    {
        if (InputAxis.magnitude > 0.1f)
        {
            thrustPower = move ? Mathf.Clamp01(InputAxis.magnitude / maxThrustDistance) : 0.0f;
            HandleInput(InputAxis);
        }
        if (InputAxis.magnitude < reachPointDistance && move)
            move = false;
        base.Update();
    }

    public void MoveToPoint(Vector3 worldPosition) 
    {
        worldPosition = new Vector3(worldPosition.x, 0.0f, worldPosition.z);
        waypointPosition = worldPosition;
        move = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(waypointPosition, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(waypointPosition, reachPointDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + flierBody.velocity);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + (flierBody.velocity) * thrustPower);
    }
}
