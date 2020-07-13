using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeCardDefend : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private Color initBacklightColor;
    private HandManager handManager;
    private DraggableArrow draggableArrow;
    private GameObject defendCardGameObj;
    private CreatureCard hoveredCard;
    private PlayerController playerController;

    public Image defendImage;
    public Sprite greyedDefend;
    public Sprite activeDefend;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        hoveredCard = (CreatureCard)GetComponent<CardDisplay>().card;
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;
        initBacklightColor = backgroundLighting.backlightingImage.color;

        if (draggableArrow.drawArrow)
        {
            Card card = draggableArrow.draggedCard.GetComponent<CardDisplay>().card;
            if (parentObjName == "Player Field" && card is NonCreatureCard && ((NonCreatureCard)card).borderColorType == NonCreatureCard.BorderColorType.defend && canDefend(card))
            {
                defendCardGameObj = handManager.hoverCopyTopCard.handTransform.gameObject;
                backgroundLighting.greenBacklighting();
            }
            else if (parentObjName == "Player Field" && ((NonCreatureCard)card).borderColorType == NonCreatureCard.BorderColorType.defend && !canDefend(card))
            {
                backgroundLighting.redBacklighting();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        defendCardGameObj = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;

        if (parentObjName == "Player Field" && defendCardGameObj)
        {
            Card defendCard = draggableArrow.draggedCard.GetComponent<CardDisplay>().card;

            activateDefend(defendCard);
            playerController.decreaseCurrEnergy(defendCard.cardCost);
            handManager.discardCard(defendCardGameObj);
        }
    }

    public void activateDefend(Card defendCard)
    {
        defendImage.sprite = activeDefend;
        hoveredCard.isDefending = true;
    }

    private bool canDefend(Card attackCard)
    {
        if (playerController.currEnergy >= attackCard.cardCost && hoveredCard.isDefending == false)
        {
            return true;
        }
        return false;
    }
}
