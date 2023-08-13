using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlayerFollower : BasicAiFlier
{
    [Header("Follow Settings")]
    public bool enableFollowing = true;
    public float followDistance = 4;
    public float updateInterval = 2;
    public Transform followTarget;
    private Vector3 FollowPosition => followTarget.position + Vector3.left * followDistance;

    private float followIntervalHit;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!followTarget || !enableFollowing)
            return;
        UpdateFollowInterval();
    }

    private void UpdateFollowInterval()
    {
        if (followIntervalHit <= Time.time)
        {
            followIntervalHit = Time.time + updateInterval;
            OnIntervalHit();
        }
    }

    private void OnIntervalHit() 
    {
        MoveToPoint(FollowPosition);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!followTarget || !enableFollowing)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(followTarget.position, FollowPosition);
        Gizmos.DrawWireSphere(FollowPosition, 0.75f);
    }
}
