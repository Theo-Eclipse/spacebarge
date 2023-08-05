using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    public float speed = 100.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followTarget)
            transform.position = Vector3.Lerp(transform.position, followTarget.position, speed * Time.deltaTime);
    }
}
