using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandCard
{
    public Transform transform;
    public float speed;
    public Transform endpointTransform;

    public HandCard(Transform transform, float speed, Transform endpointTransform)
    {
        this.transform = transform;
        this.speed = speed;
        this.endpointTransform = endpointTransform;
    }
}
