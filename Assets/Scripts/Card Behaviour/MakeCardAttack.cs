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

    public Image canAttackImage;
    public Sprite greyedAttack;
    public Sprite activeAttack;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
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
        // AttackCard attackCard = (AttackCard)draggableArrow.draggedCard.GetComponent<CardDisplay>().card;

        if (parentObjName == "Player Field" && attackCardGameObj)
        {
            canAttackImage.sprite = activeAttack;
            handManager.discardCard(attackCardGameObj);
        }
    }
}
