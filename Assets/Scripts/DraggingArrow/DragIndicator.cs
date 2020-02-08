using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicator : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    LineRenderer lr;
    private Canvas canvas;
    private int tracker;

    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("World Canvas").GetComponent<Canvas>();
        tracker = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
            }
            lr.positionCount = 2;
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            startPos = new Vector3(pos.x, pos.y, -5);
            lr.SetPosition(0, startPos);
            lr.useWorldSpace = false;
        }
        if (Input.GetMouseButton(0))
        {
            tracker++;
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            endPos = new Vector3(pos.x, pos.y, -5);
            lr.SetPosition(1, endPos);
        }
        if (Input.GetMouseButtonUp(0))
        {

        }
    }
}
