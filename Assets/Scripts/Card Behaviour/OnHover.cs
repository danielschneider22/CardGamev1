using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private RectTransform rectTransform;
    private int handPosition;
    private GameObject placeHolderGameObject;
    private static float hoverYAxisIncreaseScale = 1.8f;
    private Color initBacklightColor;

    public DragDropCard dragDropCard;
    public Canvas canvas;
    public LayoutElement layout;
    public float hoverYPositionIncrease;
    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        rectTransform = GetComponent<RectTransform>();
        layout = GetComponent<LayoutElement>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GridLayoutGroup cardGroup = eventData.pointerEnter.GetComponentInParent<GridLayoutGroup>();
        initBacklightColor = backgroundLighting.backlightingImage.color;
        backgroundLighting.hoverBacklighting();

        if (cardGroup.name == "Hand" && !Input.GetMouseButton(0))
        {
            createGroupPlaceHolder(cardGroup);
            increaseYPosition(cardGroup);
            transform.SetAsLastSibling();
            layout.ignoreLayout = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!dragDropCard.isDragging)
        {
            GridLayoutGroup cardGroup = eventData.pointerEnter.GetComponentInParent<GridLayoutGroup>();
            backgroundLighting.backlightingImage.color = initBacklightColor;

            if (cardGroup.name == "Hand")
            {
                decreaseYPosition(cardGroup);
                transform.SetSiblingIndex(handPosition);
                layout.ignoreLayout = false;
                removePlaceHolder();
            }
        } 
    }

    public void createGroupPlaceHolder(GridLayoutGroup cardGroup)
    {
        placeHolderGameObject = new GameObject("Temp", typeof(RectTransform));
        placeHolderGameObject.transform.SetParent(cardGroup.transform, false);
        handPosition = transform.GetSiblingIndex();
        placeHolderGameObject.transform.SetSiblingIndex(handPosition);
    }
    public void removePlaceHolder()
    {
        Destroy(placeHolderGameObject);
    }

    public void increaseYPosition(GridLayoutGroup cardGroup)
    {
        float cardGroupScale = cardGroup ? cardGroup.transform.localScale.x : 1f;
        hoverYPositionIncrease = ((rectTransform.rect.height * cardGroupScale * canvas.scaleFactor) / hoverYAxisIncreaseScale);
        transform.position = new Vector3(transform.position.x, transform.position.y + hoverYPositionIncrease);
    }

    public void decreaseYPosition(GridLayoutGroup cardGroup)
    {
        float cardGroupScale = cardGroup ? cardGroup.transform.localScale.x : 1f;
        transform.position = new Vector3(transform.position.x, transform.position.y - hoverYPositionIncrease);
        hoverYPositionIncrease = 0f;
    }
}
