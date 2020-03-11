using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterPlayerField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image playerFieldImage;
    private Canvas worldCanvas;
    private PlayerController playerController;
    private HandManager handManager;

    public GridLayoutGroup gridLayoutGroup;
    private void Awake()
    {
        playerFieldImage = GetComponent<Image>();
        worldCanvas = GameObject.FindGameObjectWithTag("World Canvas").GetComponent<Canvas>();
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
    }

    public void OnDrop(PointerEventData eventData)
    { 
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";
        if (draggedObject != null && parentObjName == "TopOfHandArea" && canDrop(draggedObject))
        {
            placeCardInPlayerField(draggedObject);

            // decrease cell size if too many creatures on player field
            // gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x - 10, gridLayoutGroup.cellSize.y);

            playerFieldImage.color = new Color(playerFieldImage.color.r, playerFieldImage.color.g, playerFieldImage.color.b, 0);

            playerController.decreaseCurrEnergy(draggedObject.GetComponent<CardDisplay>().card.cardCost);

            draggedObject.GetComponent<Animator>().enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // GridLayoutGroup parentGroup = eventData.pointerDrag.GetComponentInParent<GridLayoutGroup>();
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";

        if (draggedObject != null && parentObjName == "TopOfHandArea" && canDrop(draggedObject))
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.greenBacklighting();
            // playerFieldImage.color = new Color(playerFieldImage.color.r, playerFieldImage.color.g, playerFieldImage.color.b, .06f);
        } else if(draggedObject != null && parentObjName == "TopOfHandArea" && !canDrop(draggedObject))
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.redBacklighting();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";

        if (draggedObject != null && parentObjName == "TopOfHandArea")
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.whiteBacklighting();
            // playerFieldImage.color = new Color(playerFieldImage.color.r, playerFieldImage.color.g, playerFieldImage.color.b, 0);
        }
    }

    private void placeCardInPlayerField(GameObject cardObj)
    {
        setHealthBarToFlatRotation(cardObj);
        cardObj.GetComponent<DragDropCard>().canvas = worldCanvas;
        cardObj.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        GameObject newChild = Instantiate(handManager.hoverCopyTopCard.handTransform.gameObject);
        newChild.GetComponent<ToggleVisibility>().makeVisible();
        newChild.GetComponent<CanvasGroup>().blocksRaycasts = true;
        newChild.transform.SetParent(gridLayoutGroup.transform, false);
        newChild.transform.localScale = new Vector3(1f, 1f, 1);
        
        handManager.removeCardFromHand(handManager.hoverCopyTopCard.handTransform);
    }

    private void setHealthBarToFlatRotation(GameObject cardObj)
    {
        Vector3 rotationVector = cardObj.GetComponent<CardDisplay>().healthBar.GetComponent<RectTransform>().rotation.eulerAngles;
        rotationVector.x = -.2f;
        cardObj.GetComponent<CardDisplay>().healthBar.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotationVector);
    }

    private bool canDrop(GameObject cardObj)
    {
        Card droppingCard = cardObj.GetComponent<CardDisplay>().card;
        if(droppingCard.cardCost > playerController.currEnergy)
        {
            return false;
        }
        return true;
    }
}
