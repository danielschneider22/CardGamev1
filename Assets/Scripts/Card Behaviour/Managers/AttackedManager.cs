using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackedManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private DraggableArrow draggableArrow;
    private GameObject attackingCardObj;
    private CreatureCard defendingCard;
    private Animator animator;
    private AttackDefenseManager attackDefenseChangeManager;

    public HealthBar healthBar;
    public GameObject damageTextContainer;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        defendingCard = (CreatureCard)GetComponent<CardDisplay>().card;
        animator = GetComponent<Animator>();
        attackDefenseChangeManager = GetComponent<AttackDefenseManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && isValidAttack(draggableArrow.draggedCard))
        {
            backgroundLighting.greenBacklighting();

            attackingCardObj = draggableArrow.draggedCard;
            CreatureCard attackingCard = getCardAsCreatureCard(attackingCardObj);

            tempReduceHealth(attackingCard);
            attackDefenseChangeManager.tempDecreaseDefense(attackingCard.currAttack);
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
        if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && attackingCardObj != null)
        {
            CreatureCard attackingCard = getCardAsCreatureCard(attackingCardObj);
            backgroundLighting.nonselectableBacklighting();
            healthBar.restoreTempHealth();
            attackDefenseChangeManager.restoreTempDefense();
            attackingCardObj = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        backgroundLighting.nonselectableBacklighting();
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

        makeDamageTextAnimation();
        animator.enabled = true;
        string animationTrigger = shouldDestroy ? "CardDestroyedTrigger" : "CardDamagedTrigger";
        animator.SetTrigger(animationTrigger);
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

        CreatureCard attackingCardStats = (CreatureCard)attackingCard.GetComponent<CardDisplay>().card;

        if ((attackingCardStats.canAttack &&
           (attackingCardParent == "Player Field" || attackingCardParent == "Enemy Field") &&
           cardParent != attackingCardParent) && !attackingCardStats.isDestroyed && !defendingCard.isDestroyed)
        {
            return true;
        }
        return false;
    }

    private int calculateDamage(CreatureCard attackingCard)
    {
        int damage = attackingCard.currAttack - defendingCard.currDefense;
        return damage > 0 ? damage : 0;
    }

    public void destroyCard()
    {
        defendingCard.isDestroyed = true;
    }
}
