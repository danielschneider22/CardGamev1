using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Attacked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private DraggableArrow draggableArrow;
    private GameObject attackingCardObj;
    private Card defendingCard;
    private Animator animator;
    private bool validEnterTriggered;
    private AttackDefenseChangeManager attackDefenseChangeManager;

    public HealthBar healthBar;
    public TextMeshProUGUI defense;
    public GameObject damageTextContainer;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        defendingCard = GetComponent<CardDisplay>().card;
        animator = GetComponent<Animator>();
        validEnterTriggered = false;
        attackDefenseChangeManager = GetComponent<AttackDefenseChangeManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && isValidAttack(draggableArrow.draggedCard))
        {
            validEnterTriggered = true;

            attackingCardObj = draggableArrow.draggedCard;

            backgroundLighting.greenBacklighting();

            Card attackingCard = attackingCardObj.GetComponent<CardDisplay>().card;
            int damage = calculateDamage(attackingCard);
            healthBar.tempDecreaseHealth(damage);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && validEnterTriggered)
        {
            backgroundLighting.nonselectableBacklighting();
            healthBar.restoreTempHealth();
            attackingCardObj = null;
            validEnterTriggered = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (validEnterTriggered && isValidAttack(attackingCardObj))
        {
            Card attackingCard = attackingCardObj.GetComponent<CardDisplay>().card;

            backgroundLighting.nonselectableBacklighting();
            attackDefenseChangeManager.decreaseDefense(attackingCard.currAttack);
            bool shouldDestroy = healthBar.applyTempDecreaseHealth();
            GameObject damageTextObj = Instantiate(damageTextContainer, transform);
            RectTransform dmgTxtRectTransform = damageTextObj.GetComponent<RectTransform>();
            dmgTxtRectTransform.anchoredPosition = new Vector2(0, 175f);
            damageTextObj.GetComponentInChildren<Animator>().enabled = true;
            if (shouldDestroy)
            {
                animator.SetTrigger("CardDestroyedTrigger");
                //defendingCard.isDestroyed = true;
            } else
            {
                animator.SetTrigger("CardDamagedTrigger");
            }
            validEnterTriggered = false;
        }
    }

    private bool isValidAttack(GameObject attackingCard)
    {
        GridLayoutGroup cardGroup = GetComponentInParent<GridLayoutGroup>();
        GridLayoutGroup attackingCardGroup = attackingCard.GetComponentInParent<GridLayoutGroup>();
        Card attackingCardStats = attackingCard.GetComponent<CardDisplay>().card;
        if (((cardGroup.name == "Player Field" || cardGroup.name == "Enemy Field") &&
           (attackingCardGroup.name == "Player Field" || attackingCardGroup.name == "Enemy Field") &&
           cardGroup.name != attackingCardGroup.name) && !attackingCardStats.isDestroyed && !defendingCard.isDestroyed)
        {
            return true;
        }
        return false;
    }

    private int calculateDamage(Card attackingCard)
    {
        int damage = attackingCard.currAttack - defendingCard.currDefense;
        return damage > 0 ? damage : 0;
    }

    public void destroyCard()
    {
        defendingCard.isDestroyed = true;
    }
}
