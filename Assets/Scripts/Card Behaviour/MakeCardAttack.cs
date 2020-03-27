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
    private AttackCard attackCard;

    public Image canAttackImage;

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
            attackCard = (AttackCard)draggableArrow.draggedCard.GetComponent<CardDisplay>().card;
            backgroundLighting.greenBacklighting();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        attackCard = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        string parentObjName = gameObject.transform.parent.name;

        if (parentObjName == "Player Field" && attackCard)
        {
            canAttackImage.color = Color.green;
        }
    }
}
