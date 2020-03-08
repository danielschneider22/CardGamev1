using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HoverCopyTopCard
{
    public Transform copyTransform;
    public Transform handTransform;

    public HoverCopyTopCard(Transform copyTransform, Transform handTransform)
    {
        this.copyTransform = copyTransform;
        this.handTransform = handTransform;
    }
}
