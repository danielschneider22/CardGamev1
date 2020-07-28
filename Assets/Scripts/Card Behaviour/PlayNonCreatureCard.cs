using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayNonCreatureCard : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private Color initBacklightColor;
    private HandManager handManager;
    private DraggableArrow draggableArrow;
    private GameObject draggingGameObject;
    private Card hoveredCard;
    private PlayerController playerController;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        hoveredCard = GetComponent<CardDisplay>().card;
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;
        initBacklightColor = backgroundLighting.backlightingImage.color;

        if (draggableArrow.drawArrow && draggableArrow.draggedCard.GetComponent<CardDisplay>().card is NonCreatureCard && parentObjName == "Player Field")
        {
            NonCreatureCard card = (NonCreatureCard)draggableArrow.draggedCard.GetComponent<CardDisplay>().card;
            bool isPlayable = CanPlayManager.canPlay(card.canPlayRequirements, gameObject, card, playerController);

            if (isPlayable)
            {

                draggingGameObject = handManager.hoverCopyTopCard.handTransform.gameObject;
                backgroundLighting.greenBacklighting();
            }
            else if (!isPlayable)
            {
                backgroundLighting.redBacklighting();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        draggingGameObject = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;

        if (parentObjName == "Player Field" && draggingGameObject)
        {
            NonCreatureCard card = (NonCreatureCard)draggableArrow.draggedCard.GetComponent<CardDisplay>().card;

            NonCreatureEffectsManager.enactNonCreatureEffect(card.effects, gameObject, playerController);
            playerController.decreaseCurrEnergy(card.cardCost);
            handManager.discardCard(draggingGameObject);
        }
    }
}
