using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CardDisplay;

public class EnterPlayerDiscardArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image playerDiscardImage;
    private Canvas screenSpaceOverlayCanvas;
    private PlayerController playerController;
    private HandManager handManager;
    private FieldManager playerFieldManager;
    public DiscardManager discardManager;
    public PlayerDeckManager deckManager;

    public GameObject cardTemplate;
    private void Awake()
    {
        playerDiscardImage = GetComponent<Image>();
        screenSpaceOverlayCanvas = GameObject.FindGameObjectWithTag("Screen Space Canvas").GetComponent<Canvas>();
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
        playerFieldManager = GameObject.FindGameObjectWithTag("Player Field Manager").GetComponent<FieldManager>();
    }

    public void OnDrop(PointerEventData eventData)
    { 
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";
        if (draggedObject != null && (parentObjName == "TopOfHandArea" || parentObjName == "Player Field") && canDrop(draggedObject))
        {
            placeCardInDiscardArea(draggedObject);
            deckManager.drawCard();

            playerDiscardImage.color = new Color(playerDiscardImage.color.r, playerDiscardImage.color.g, playerDiscardImage.color.b, 0);

            playerController.decreaseCurrEnergy(((CreatureCard)draggedObject.GetComponent<CardDisplay>().card).retreatCost);

            draggedObject.GetComponent<Animator>().enabled = false;

            
            if(draggedObject.GetComponent<CardDisplay>().location == Location.field)
            {
                playerFieldManager.removeCardFromField(draggedObject.transform);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";

        if (draggedObject != null && (parentObjName == "TopOfHandArea" || parentObjName == "Player Field") && canDrop(draggedObject))
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.greenBacklighting();
            // playerFieldImage.color = new Color(playerFieldImage.color.r, playerFieldImage.color.g, playerFieldImage.color.b, .06f);
        } else if(draggedObject != null && (parentObjName == "TopOfHandArea" || parentObjName == "Player Field") && !canDrop(draggedObject))
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.redBacklighting();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        string parentObjName = draggedObject != null ? draggedObject.transform.parent.name : "";

        if (draggedObject != null && (parentObjName == "TopOfHandArea" || parentObjName == "Player Field"))
        {
            ChangeBackgroundLighting backgroundLighting = eventData.pointerDrag.GetComponent<ChangeBackgroundLighting>();
            backgroundLighting.whiteBacklighting();
            // playerFieldImage.color = new Color(playerFieldImage.color.r, playerFieldImage.color.g, playerFieldImage.color.b, 0);
        }
    }

    private void placeCardInDiscardArea(GameObject cardObj)
    {
        cardTemplate.SetActive(false);
        GameObject newChild = Instantiate(cardTemplate);
        // setHealthBarToFlatRotation(newChild);
        newChild.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
        newChild.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
        newChild.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        newChild.GetComponent<CardDisplay>().card = cardObj.GetComponent<CardDisplay>().card;
        newChild.GetComponent<CardDisplay>().material = cardObj.GetComponent<CardDisplay>().material;
        newChild.GetComponent<CardDisplay>().location = Location.discard;
        newChild.GetComponent<ToggleVisibility>().makeVisible();
        newChild.GetComponent<CanvasGroup>().blocksRaycasts = true;
        newChild.transform.localScale = cardObj.transform.localScale;
        newChild.SetActive(true);

        newChild.transform.position = cardObj.transform.position;
        newChild.transform.GetComponent<RectTransform>().anchoredPosition = cardObj.GetComponent<RectTransform>().anchoredPosition;

        discardManager.addCardToDiscardArea(newChild, cardObj.GetComponent<CardDisplay>().location == Location.hand);
        cardTemplate.SetActive(true);

        if (cardObj.GetComponent<CardDisplay>().location == Location.hand)
        {
            handManager.removeCardFromHand(handManager.hoverCopyTopCard.handTransform);
        }
    }

    private bool canDrop(GameObject cardObj)
    {
        CardDisplay cardDisplay = cardObj.GetComponent<CardDisplay>();
        if (!(cardDisplay.card is CreatureCard))
        {
            return false;
        }
        CreatureCard droppingCard = (CreatureCard)cardObj.GetComponent<CardDisplay>().card;
        if(droppingCard.retreatCost > playerController.currEnergy || (!droppingCard.energized && cardDisplay.location == Location.field))
        {
            return false;
        }
        return true;
    }
}
