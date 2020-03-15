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
    private FieldManager playerFieldManager;

    public GameObject creatureCardTemplate;
    private void Awake()
    {
        playerFieldImage = GetComponent<Image>();
        worldCanvas = GameObject.FindGameObjectWithTag("World Canvas").GetComponent<Canvas>();
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
        playerFieldManager = GameObject.FindGameObjectWithTag("Player Field Manager").GetComponent<FieldManager>();
    }

    public void OnDrop(PointerEventData eventData)
    { 
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";
        if (draggedObject != null && parentObjName == "TopOfHandArea" && canDrop(draggedObject))
        {
            placeCardInPlayerField(draggedObject);

            playerFieldImage.color = new Color(playerFieldImage.color.r, playerFieldImage.color.g, playerFieldImage.color.b, 0);

            playerController.decreaseCurrEnergy(draggedObject.GetComponent<CardDisplay>().card.cardCost);

            draggedObject.GetComponent<Animator>().enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
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
        creatureCardTemplate.SetActive(false);
        GameObject newChild = Instantiate(creatureCardTemplate);
        // setHealthBarToFlatRotation(newChild);
        newChild.GetComponent<DragDropCard>().canvas = worldCanvas;
        newChild.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        newChild.GetComponent<CardDisplay>().card = cardObj.GetComponent<CardDisplay>().card;
        newChild.GetComponent<CardDisplay>().material = cardObj.GetComponent<CardDisplay>().material;
        newChild.GetComponent<CardDisplay>().location = "field";
        newChild.GetComponent<ToggleVisibility>().makeVisible();
        newChild.GetComponent<CanvasGroup>().blocksRaycasts = true;
        newChild.transform.localScale = new Vector3(.65f, .65f, 1);
        newChild.SetActive(true);

        float halfHeight = newChild.GetComponent<RectTransform>().rect.height / 2;
        newChild.transform.position = playerFieldImage.transform.position;// Camera.main.ScreenToWorldPoint(cardObj.transform.position); // new Vector3(Input.mousePosition.x, Input.mousePosition.y - halfHeight, Input.mousePosition.z);
        playerFieldManager.addCardToField(newChild);
        creatureCardTemplate.SetActive(true);

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
