﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropCard : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private CanvasGroup canvasGroup;
    private CanvasGroup playerFieldCanvasGroup;
    private ChangeBackgroundLighting backgroundLighting;
    private LayoutElement layout;
    private GridLayoutGroup cardGroup;
    private OnHover onHover;
    private CardDisplay cardDisplay;
    private DraggableArrow draggableArrow;
    private HandManager handManager;
    private float pauseBeforeResettingHand;

    public Canvas canvas;
    public bool isDragging;
    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        onHover = GetComponent<OnHover>();
        playerFieldCanvasGroup = GameObject.FindGameObjectWithTag("Player Field").GetComponent<CanvasGroup>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        layout = GetComponent<LayoutElement>();
        cardDisplay = GetComponent<CardDisplay>();
        cardGroup = GetComponentInParent<GridLayoutGroup>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
    }

    public void Update()
    {
        if(pauseBeforeResettingHand > 0f)
        {
            pauseBeforeResettingHand -= Time.deltaTime;
            if(pauseBeforeResettingHand <= 0f)
            {
                pauseBeforeResettingHand = 0f;
                handManager.resetHandPositions();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!draggableArrow.drawArrow && transform.parent.name != "Enemy Field")
        {
            rectTransform.anchoredPosition += eventData.delta / getCardScaling(eventData);
            if(handManager.hoverCopyTopCard != null)
            {
                handManager.hoverCopyTopCard.handTransform.GetComponent<RectTransform>().anchoredPosition += eventData.delta / getCardScaling(eventData);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        storeCardPropertiesAtMouseClick();
        if (transform.parent.name == "TopOfHandArea" && cardDisplay.card.cardType == "Creature")
        {
            toggleCardDragProperites(true);
            setCardPositionToMousePointer();
            togglePlayerFieldInteractable(true);
            handManager.clearTopCardFromMovingCards();
        } else if (transform.parent.name == "Player Field")
        {
            Vector3 cardPosition = transform.parent.name == "TopOfHandArea" ? transform.position : Input.mousePosition; // canvas.worldCamera.WorldToScreenPoint(transform.position);
            this.isDragging = true;
            draggableArrow.startPos = cardPosition;
            draggableArrow.drawArrow = true;
            draggableArrow.draggedCard = gameObject;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        restoreCardPropertiesToMouseClickState();
        layout.ignoreLayout = false;
        if (transform.parent.name == "TopOfHandArea" && cardDisplay.card.cardType == "Creature")
        {
            toggleCardDragProperites(false);
            togglePlayerFieldInteractable(false);
            // give time for drop actions to be done before resetting hand positions
            pauseBeforeResettingHand = .01f;
        } else if (transform.parent.name == "Player Field")
        {
            this.isDragging = false;
            draggableArrow.drawArrow = false;
            backgroundLighting.selectableBacklighting();
        }
    }

    /*Helper Functions*/
    public void toggleCardDragProperites(bool isDragging)
    {
        canvasGroup.blocksRaycasts = !isDragging;
        if (isDragging) {
            backgroundLighting.whiteBacklighting();
        }
        else {
            backgroundLighting.selectableBacklighting();
        };

        this.isDragging = isDragging;
        layout.ignoreLayout = isDragging;
    }
    private void storeCardPropertiesAtMouseClick()
    {
        initialScale = transform.localScale;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    private void restoreCardPropertiesToMouseClickState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
    private void togglePlayerFieldInteractable(bool canInteract)
    {
        playerFieldCanvasGroup.blocksRaycasts = canInteract;
    }
    private float getCardScaling(PointerEventData eventData)
    {
        // GridLayoutGroup cardGroup = eventData.pointerDrag.GetComponentInParent<GridLayoutGroup>();
        // float cardGroupScale = cardGroup ? cardGroup.transform.localScale.x : 1f;
        return canvas.scaleFactor; //* transform.localScale.x;
    }
    private void setCardPositionToMousePointer()
    {
        /* Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos); */
        RectTransform cardRectTransform = GetComponent<RectTransform>();
        float halfHeight = cardRectTransform.rect.height / 2;

        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - halfHeight, Input.mousePosition.z);
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        handManager.hoverCopyTopCard.handTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

}
