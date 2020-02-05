using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropCard : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    public Canvas canvas;
    public OnHover onHover;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private CanvasGroup canvasGroup;
    private CanvasGroup playerFieldCanvasGroup;
    private ChangeBackgroundLighting backgroundLighting;
    public bool isDragging;
    private LayoutElement layout;
    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        onHover = GetComponent<OnHover>();
        playerFieldCanvasGroup = GameObject.FindGameObjectWithTag("Player Field").GetComponent<CanvasGroup>();
        layout = GetComponent<LayoutElement>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / getCardScaling(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toggleCardDragProperites(true);
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        setCardPositionToMousePointer();
        togglePlayerFieldInteractable(true);
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onHover.removePlaceHolder();
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        toggleCardDragProperites(false);
        togglePlayerFieldInteractable(false);
    }

    /*Helper Functions*/
    public void toggleCardDragProperites(bool isDragging)
    {
        canvasGroup.blocksRaycasts = !isDragging;
        canvasGroup.alpha = isDragging ? .8f : 1f;
        if (isDragging) { backgroundLighting.whiteBacklighting(); }
        else { backgroundLighting.blackBacklighting(); };
        this.isDragging = isDragging;
        layout.ignoreLayout = isDragging;
    }
    public void togglePlayerFieldInteractable(bool canInteract)
    {
        playerFieldCanvasGroup.blocksRaycasts = canInteract;
    }
    private float getCardScaling(PointerEventData eventData)
    {
        GridLayoutGroup cardGroup = eventData.pointerDrag.GetComponentInParent<GridLayoutGroup>();
        float cardGroupScale = cardGroup ? cardGroup.transform.localScale.x : 1f;
        return canvas.scaleFactor * cardGroupScale;
    }
    private void setCardPositionToMousePointer()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

}
