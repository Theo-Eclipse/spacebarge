using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAiFlier : BasicFlier
{
    [Space, Header("AI Waypoint")]
    public Transform checkDist;
    public Vector3 destinationPoint = Vector3.zero;
    public Vector3 wayPoint = Vector3.zero;
    public float maxThrustDistance = 12.5f;
    public float reachPointDistance = 0.5f;
    public float minimumThrust = 0.2f;
    public bool move = false;

    [Space, Header("Obstacle Avoidance")]
    [Range(4, 24)]public int raysCount = 6;
    public float spreadAngle = 60.0f;
    public float castDistance = 10.0f;
    public float castRadius = 1.4f;
    public LayerMask obstacleLayerMask;
    public float checkInterval = 1.0f;
    public float castDirOffset = 0.0f;
    public float testAngle = 0;

    private int rayIndex = 0;
    private RaycastHit hit;
    private Ray sphereCastRay;
    private Vector3 bestResult;

    private Vector2 InputAxis => new Vector2(wayPoint.x - transform.position.x, wayPoint.z - transform.position.z);
    private Vector3 LastCastPosition => transform.position + sphereCastRay.direction.normalized * castDistance;
    private float TargetThrust()
    {
        if (lockAtTarget)
            return Mathf.Clamp(InputAxis.magnitude / maxThrustDistance, minimumThrust, 1.0f);
        else if (Mathf.Abs(AngleToAxisInput) < 5.0f)
            return Mathf.Clamp(InputAxis.magnitude / maxThrustDistance, minimumThrust, 1.0f);
        else return minimumThrust;
    }
    private float intervalHit = 0;

    // Update is called once per frame
    protected override void Update()
    {
        if (InputAxis.magnitude > 0.1f)
        {
            HandleInput(InputAxis);
            thrustPower = move ? TargetThrust() : 0.0f;
        }
        if (Vector3.Distance(wayPoint, destinationPoint) > 0.5f)
            UpdateWayPointRealtime();
        if (move && Vector3.Distance(wayPoint, destinationPoint) < 0.5f && InputAxis.magnitude < reachPointDistance)
            OnWayPointReached();
        base.Update();
    }

    private void UpdateWayPointRealtime() 
    {
        if (move && intervalHit <= Time.time) 
        {
            intervalHit = Time.time + checkInterval;
            UpdateWayPoint();
        }
    }

    private void OnWayPointReached() 
    {
        move = false;
    }

    private void UpdateWayPoint() 
    {
        wayPoint = destinationPoint - transform.position;
        if (CastTest(wayPoint.normalized))
            wayPoint = GetAvoidObstaclePoint(destinationPoint);
        else
            wayPoint = wayPoint.magnitude < castDistance ? transform.position + wayPoint : transform.position + wayPoint.normalized * castDistance;
        move = true;
    }

    private Vector3 GetAvoidObstaclePoint(Vector3 finalDestination) 
    {
        float distanceCache = float.MaxValue;
        Vector3 resultPoint = Vector3.zero;
        for (rayIndex = 0; rayIndex < raysCount; rayIndex++)
        {
            if (CastTest(AngleToDirection(-spreadAngle * 0.5f + (spreadAngle / raysCount) * rayIndex)))
                continue;// we want to contine to find a way, without obstacles.
            if (Vector3.Distance(LastCastPosition, finalDestination) < distanceCache)
            {
                resultPoint = LastCastPosition;
                distanceCache = Vector3.Distance(LastCastPosition, finalDestination);
            }
        }
        if (resultPoint != Vector3.zero)
            return resultPoint;

        if (!CastTest(-transform.forward))// We are stuck. let's try going back.
            return LastCastPosition;
        else 
            return (finalDestination - transform.position).normalized * castDistance;// no way back. let's try to push that obstacle.
    }

    public void MoveToPoint(Vector3 worldPosition) 
    {
        destinationPoint = worldPosition;
        UpdateWayPoint();
    }



    public bool CastTest(Vector3 direction) 
    {
        sphereCastRay = new Ray(transform.position + castDirOffset * direction.normalized, direction);
        return Physics.SphereCast(sphereCastRay, castRadius, out hit, castDistance, obstacleLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wayPoint, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, wayPoint);
        Gizmos.DrawWireSphere(wayPoint, reachPointDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + flierBody.velocity);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + (flierBody.velocity) * thrustPower);
        DrawSphereCastGizmo();
    }

    private Vector3 AngleToDirection(float angle)
    {
        angle += Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
    }

    private void DrawSphereCastGizmo() 
    {
        for (rayIndex = 0; rayIndex < raysCount; rayIndex++)
            if (CastTest(AngleToDirection(-spreadAngle*0.5f + (spreadAngle/raysCount) * rayIndex)))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(sphereCastRay.origin, sphereCastRay.direction.normalized * hit.distance);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(sphereCastRay.origin + sphereCastRay.direction.normalized * hit.distance, castRadius);
                Gizmos.DrawRay(sphereCastRay.origin + sphereCastRay.direction.normalized * hit.distance, sphereCastRay.direction.normalized * (castDistance- hit.distance));
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(sphereCastRay.origin, sphereCastRay.direction.normalized * castDistance);
                Gizmos.DrawWireSphere(sphereCastRay.origin + sphereCastRay.direction.normalized * castDistance, castRadius);
            }
    }
}
