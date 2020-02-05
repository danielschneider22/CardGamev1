using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHandHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    public Canvas canvas;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // transform.position = new Vector3(transform.position.x, transform.position.y + ((rectTransform.rect.height * canvas.scaleFactor)) / 10f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // transform.position = new Vector3(transform.position.x, transform.position.y - ((rectTransform.rect.height * canvas.scaleFactor)) / 10f);
    }
}
