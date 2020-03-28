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
    private CreatureCard defendingCard;
    private Animator animator;
    private bool validEnterTriggered;
    private AttackDefenseChangeManager attackDefenseChangeManager;

    public HealthBar healthBar;
    public TextMeshProUGUI defense;
    public GameObject damageTextContainer;
    public Sprite greyedAttack;
    public Sprite activeAttack;
    public Image attackImage;

    private void Awake()
    {
        backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        draggableArrow = GameObject.FindGameObjectWithTag("Draggable Arrow").GetComponent<DraggableArrow>();
        defendingCard = (CreatureCard) GetComponent<CardDisplay>().card;
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

            CreatureCard attackingCard = (CreatureCard)attackingCardObj.GetComponent<CardDisplay>().card;
            int damage = calculateDamage(attackingCard);
            healthBar.tempDecreaseHealth(damage);
        } else if (draggableArrow.drawArrow && draggableArrow.draggedCard != gameObject && !isValidAttack(draggableArrow.draggedCard))
        {
            backgroundLighting.redBacklighting();
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
            CreatureCard attackingCard = (CreatureCard) attackingCardObj.GetComponent<CardDisplay>().card;
            attackingCard.canAttack = false;
            attackingCardObj.GetComponent<CardDisplay>().attackImage.sprite = greyedAttack;

            backgroundLighting.nonselectableBacklighting();
            attackDefenseChangeManager.decreaseDefense(attackingCard.currAttack);
            bool shouldDestroy = healthBar.applyTempDecreaseHealth();
            GameObject damageTextObj = Instantiate(damageTextContainer, transform);
            RectTransform dmgTxtRectTransform = damageTextObj.GetComponent<RectTransform>();
            dmgTxtRectTransform.anchoredPosition = new Vector2(0, 30f);
            damageTextObj.GetComponentInChildren<Animator>().enabled = true;
            damageTextObj.GetComponentInChildren<TextMeshProUGUI>().text = healthBar.tempDamage.ToString();
            damageTextObj.GetComponentInChildren<TextMeshProUGUI>().fontSize = 32f;
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
        if(!(attackingCard.GetComponent<CardDisplay>().card is CreatureCard)) { return false; }
        string cardGroup = transform.parent.name;
        string attackingCardGroup = attackingCard.transform.parent.name;
        CreatureCard attackingCardStats = (CreatureCard) attackingCard.GetComponent<CardDisplay>().card;
        if (((cardGroup == "Player Field" || cardGroup == "Enemy Field") &&
            attackingCardStats.canAttack &&
           (attackingCardGroup == "Player Field" || attackingCardGroup == "Enemy Field") &&
           cardGroup != attackingCardGroup) && !attackingCardStats.isDestroyed && !defendingCard.isDestroyed)
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
