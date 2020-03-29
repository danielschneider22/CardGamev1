using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeCardAttack : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private Color initBacklightColor;
    private HandManager handManager;
    private DraggableArrow draggableArrow;
    private GameObject attackCardGameObj;
    private CreatureCard hoveredCard;
    private PlayerController playerController;

    public Image attackImage;
    public Sprite greyedAttack;
    public Sprite activeAttack;

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

        if (parentObjName == "Player Field" && draggableArrow.drawArrow && draggableArrow.draggedCard.GetComponent<CardDisplay>().card.cardType == "Attack")
        {
            attackCardGameObj = handManager.hoverCopyTopCard.handTransform.gameObject;
            backgroundLighting.greenBacklighting();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        attackCardGameObj = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;

        if (parentObjName == "Player Field" && attackCardGameObj)
        {
            AttackCard attackCard = (AttackCard)draggableArrow.draggedCard.GetComponent<CardDisplay>().card;

            attackImage.sprite = activeAttack;
            hoveredCard.canAttack = true;
            playerController.decreaseCurrEnergy(attackCard.cardCost);
            handManager.discardCard(attackCardGameObj);
        }
    }
}
