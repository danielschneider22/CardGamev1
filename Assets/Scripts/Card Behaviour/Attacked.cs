using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Attacked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private DraggableArrow draggableArrow;
    private GameObject attackingCardObj;
    private Card defendingCard;

    public HealthBar healthBar;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        defendingCard = GetComponent<CardDisplay>().card;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && isValidAttack(draggableArrow.draggedCard))
        {
            attackingCardObj = draggableArrow.draggedCard;

            backgroundLighting.greenBacklighting();

            Card attackingCard = attackingCardObj.GetComponent<CardDisplay>().card;
            int damage = calculateDamage(attackingCard);
            healthBar.tempDecreaseHealth(damage);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject)
        {
            backgroundLighting.nonselectableBacklighting();
            healthBar.restoreTempHealth();
            attackingCardObj = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        backgroundLighting.nonselectableBacklighting();
        bool shouldDestroy = healthBar.applyTempDecreaseHealth();
        if(shouldDestroy)
        {
            defendingCard.isDestroyed = true;
        }
    }

    private bool isValidAttack(GameObject attackingCard)
    {
        GridLayoutGroup cardGroup = GetComponentInParent<GridLayoutGroup>();
        GridLayoutGroup draggedCardGroup = attackingCard.GetComponentInParent<GridLayoutGroup>();
        if(cardGroup.name == "Player Field" || cardGroup.name == "Enemy Field" &&
           draggedCardGroup.name == "Player Field" || draggedCardGroup.name == "Enemy Field" &&
           cardGroup.name != draggedCardGroup.name)
        {
            return true;
        }
        return false;
    }

    private int calculateDamage(Card attackingCard)
    {
        int damage = attackingCard.attack - defendingCard.defense;
        return damage > 0 ? damage : 0;
    }
}
