using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private RectTransform rectTransform;
    public DragDropCard dragDropCard;
    public Canvas canvas;
    public LayoutElement layout;
    private int handPosition;
    private GameObject placeHolderGameObject;
    private float hoverIncreaseAmount;
    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        rectTransform = GetComponent<RectTransform>();
        layout = GetComponent<LayoutElement>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundLighting.blueBacklighting();
        if(eventData.pointerEnter.GetComponentInParent<GridLayoutGroup>().name == "Hand")
        {
            GridLayoutGroup cardGroup = eventData.pointerEnter.GetComponentInParent<GridLayoutGroup>();
            float cardGroupScale = cardGroup ? cardGroup.transform.localScale.x : 1f;
            hoverIncreaseAmount = ((rectTransform.rect.height * cardGroupScale * canvas.scaleFactor) / 2f);
            transform.position = new Vector3(transform.position.x, transform.position.y + hoverIncreaseAmount);
            handPosition = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            layout.ignoreLayout = true;

            placeHolderGameObject = new GameObject("Temp", typeof(RectTransform));
            placeHolderGameObject.transform.SetParent(cardGroup.transform, false);
            placeHolderGameObject.transform.SetSiblingIndex(handPosition);
        }
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - hoverIncreaseAmount);
        hoverIncreaseAmount = 0f;
        Destroy(placeHolderGameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!dragDropCard.isDragging)
        {
            backgroundLighting.blackBacklighting();
            if (eventData.pointerEnter.GetComponentInParent<GridLayoutGroup>().name == "Hand")
            {
                GridLayoutGroup cardGroup = eventData.pointerEnter.GetComponentInParent<GridLayoutGroup>();
                float cardGroupScale = cardGroup ? cardGroup.transform.localScale.x : 1f;
                transform.position = new Vector3(transform.position.x, transform.position.y - hoverIncreaseAmount);
                hoverIncreaseAmount = 0f;
                transform.SetSiblingIndex(handPosition);
                layout.ignoreLayout = false;
                Destroy(placeHolderGameObject);
            }
        }
        
    }
}
