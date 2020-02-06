using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterPlayerField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    private Image image;
    private Canvas worldCanvas;
    private void Awake()
    {
        image = GetComponent<Image>();
        worldCanvas = GameObject.FindGameObjectWithTag("World Canvas").GetComponent<Canvas>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponentInParent<GridLayoutGroup>().name == "Hand")
        {
            GameObject draggedObject = eventData.pointerDrag;
            draggedObject.transform.localScale = new Vector3(1f, 1f, 1);
            // draggedObject.transform.rotation = new Quaternion(.1f, 0f, 0f, 1f);
            draggedObject.GetComponent<DragDropCard>().canvas = worldCanvas;
            GameObject newChild = Instantiate(draggedObject);

            // gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x - 10, gridLayoutGroup.cellSize.y);

            newChild.transform.SetParent(gridLayoutGroup.transform, false);
            Destroy(eventData.pointerDrag);
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponentInParent<GridLayoutGroup>().name == "Hand")
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.greenBacklighting();
            image.color = new Color(image.color.r, image.color.g, image.color.b, .06f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponentInParent<GridLayoutGroup>().name == "Hand")
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.whiteBacklighting();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }
}
