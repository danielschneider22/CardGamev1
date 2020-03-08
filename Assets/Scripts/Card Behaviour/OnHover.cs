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
    private HandManager handManager;

    public DragDropCard dragDropCard;
    public Canvas canvas;
    public LayoutElement layout;
    public float hoverYPositionIncrease;
    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        rectTransform = GetComponent<RectTransform>();
        layout = GetComponent<LayoutElement>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;
        initBacklightColor = backgroundLighting.backlightingImage.color;
        backgroundLighting.hoverBacklighting();

        if (parentObjName == "Hand" && !Input.GetMouseButton(0))
        {
            handManager.hoverCard(gameObject.transform);
            // increaseYPosition(cardGroup);
            // transform.SetAsLastSibling();
            // layout.ignoreLayout = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!dragDropCard.isDragging)
        {
            string parentObjName = gameObject.transform.parent.name;
            backgroundLighting.backlightingImage.color = initBacklightColor;

            if (parentObjName == "TopOfHandArea")
            {
                handManager.resetHandPositions(500);
                // decreaseYPosition(cardGroup);
                // transform.SetSiblingIndex(handPosition);
                // layout.ignoreLayout = false;
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
