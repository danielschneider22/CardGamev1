using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackedPlayerManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public ChangeBackgroundLighting backgroundLighting;
    private DraggableArrow draggableArrow;
    public GameObject attackingCardObj;
    public PlayerController playerController;

    public HealthBarPlayer healthBar;
    public GameObject damageTextContainer;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && isValidAttack(draggableArrow.draggedCard))
        {
            backgroundLighting.greenBacklighting();

            attackingCardObj = draggableArrow.draggedCard;
            CreatureCard attackingCard = getCardAsCreatureCard(attackingCardObj);

            tempReduceHealth(attackingCard);
        }
        else if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && !isValidAttack(draggableArrow.draggedCard))
        {
            backgroundLighting.redBacklighting();
        }
    }

    public void tempReduceHealth(CreatureCard attackingCard)
    {
        int damage = calculateDamage(attackingCard);
        healthBar.tempDecreaseHealth(damage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundLighting.transparentBacklighting();
        if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && attackingCardObj != null)
        {
            CreatureCard attackingCard = getCardAsCreatureCard(attackingCardObj);
       
            healthBar.restoreTempHealth();
            attackingCardObj = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        backgroundLighting.transparentBacklighting();
        if (attackingCardObj != null && isValidAttack(attackingCardObj))
        {
            CreatureCard attackingCard = getCardAsCreatureCard(attackingCardObj);

            attackingCardCantAttack();

            applyTempAttack(attackingCard);
        }
    }

    public void applyTempAttack(CreatureCard attackingCard)
    {
        // attackDefenseChangeManager.decreaseDefense(attackingCard.currAttack);
        bool shouldDestroy = healthBar.applyTempDecreaseHealth();

        EnergizedManager energizedManager = attackingCardObj.GetComponent<EnergizedManager>();
        energizedManager.deenergize();

        makeDamageTextAnimation();
        attackingCardObj = null;
    }
    private void makeDamageTextAnimation()
    {
        GameObject damageTextObj = Instantiate(damageTextContainer, transform);
        RectTransform dmgTxtRectTransform = damageTextObj.GetComponent<RectTransform>();
        dmgTxtRectTransform.anchoredPosition = new Vector2(0, 30f);
        damageTextObj.GetComponentInChildren<Animator>().enabled = true;
        damageTextObj.GetComponentInChildren<TextMeshProUGUI>().text = healthBar.tempDamage.ToString();
        damageTextObj.GetComponentInChildren<TextMeshProUGUI>().fontSize = 32f;
    }
    private void attackingCardCantAttack()
    {
        AttackDefenseManager atckDefManager = attackingCardObj.GetComponent<AttackDefenseManager>();
        atckDefManager.cantAttack();
    }
    private bool isCreatureCard(GameObject attackingCard)
    {
        return attackingCard.GetComponent<CardDisplay>().card is CreatureCard;
    }

    private CreatureCard getCardAsCreatureCard(GameObject cardObj)
    {
        return (CreatureCard)cardObj.GetComponent<CardDisplay>().card;
    }

    private bool isValidAttack(GameObject attackingCard)
    {
        if (!(isCreatureCard(attackingCard))) { return false; }

        string cardParent = transform.parent.name;
        string attackingCardParent = attackingCard.transform.parent.name;

        CardDisplay cardDisplay = attackingCard.GetComponent<CardDisplay>();
        CreatureCard attackingCardStats = (CreatureCard)cardDisplay.card;
        

        if ((attackingCardStats.canAttack &&
           (attackingCardParent == "Player Field" || attackingCardParent == "Enemy Field") &&
           cardParent != attackingCardParent) && !attackingCardStats.isDestroyed &&
           !playerController.isDefended() && (cardDisplay.location == CardDisplay.Location.field && playerController.isEnemyController || cardDisplay.location == CardDisplay.Location.enemyField && !playerController.isEnemyController))
        {
            return true;
        }
        return false;
    }

    private int calculateDamage(CreatureCard attackingCard)
    {
        int damage = attackingCard.currAttack;
        return damage > 0 ? damage : 0;
    }
}
