using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovingCard
{
    public Transform transform;
    public float speed;
    public Transform endpointTransform;
    public Vector3 endpointScale;

    public MovingCard(Transform transform, float speed, Transform endpointTransform)
    {
        this.transform = transform;
        this.speed = speed;
        this.endpointTransform = endpointTransform;
    }

    public MovingCard(Transform transform, float speed, Transform endpointTransform, Vector3 endpointScale)
    {
        this.transform = transform;
        this.speed = speed;
        this.endpointTransform = endpointTransform;
        this.endpointScale = endpointScale;
    }
}
