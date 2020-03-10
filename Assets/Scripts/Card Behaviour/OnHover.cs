using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private Color initBacklightColor;
    private HandManager handManager;

    public DragDropCard dragDropCard;
    public Canvas canvas;
    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;
        initBacklightColor = backgroundLighting.backlightingImage.color;
        backgroundLighting.hoverBacklighting();

        if (parentObjName == "Hand" && !Input.GetMouseButton(0) && !eventData.dragging)
        {
            handManager.hoverCard(gameObject.transform);
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
            }
        } 
    }
}
