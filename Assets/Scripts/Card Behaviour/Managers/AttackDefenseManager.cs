using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackDefenseManager : MonoBehaviour
{
    private CreatureCard card;
    private CardDisplay cardDisplay;
    private Color zeroStatColor = new Color(0.6415094f, 0.2935208f, 0.2935208f);

    public TextMeshProUGUI defenseText;
    public int defenseBeforeChange;
    public bool isDefendingBeforeChange;
    public Sprite brokenShieldSprite;
    public Sprite shieldSprite;
    public Sprite attackSprite;
    public Sprite attackAndDefendSprite;

    private void Awake()
    {
        card = (CreatureCard) GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
    }
    public void decreaseDefense(int amountToDecrease)
    {
        card.currDefense = card.currDefense - amountToDecrease;
        if(card.currDefense <= 0)
        {
            card.currDefense = 0;
            setShieldBroken();
        }
        defenseText.text = card.currDefense.ToString();
    }

    public void tempDecreaseDefense(int amountToDecrease)
    {
        defenseBeforeChange = card.currDefense;
        isDefendingBeforeChange = card.isDefending;
        decreaseDefense(amountToDecrease);
    }

    public void restoreTempDefense()
    {
        card.currDefense = defenseBeforeChange;
        if(isDefendingBeforeChange)
        {
            setIsDefending();
        }
        defenseText.text = card.currDefense.ToString();
    }

    public void increaseDefense(int amountToIncrease)
    {
        card.currDefense = card.currDefense + amountToIncrease;
        defenseText.text = card.currDefense.ToString();
    }

    public void cantAttack()
    {
        card.canAttack = false;
        if(!card.isDefending)
        {
            cardDisplay.attackingDefendingImage.enabled = false;
        } else
        {
            cardDisplay.attackingDefendingImage.sprite = shieldSprite;
        }
    }
    public void canAttack()
    {
        card.canAttack = true;
        cardDisplay.attackingDefendingImage.enabled = true;
        if (card.isDefending)
        {
            cardDisplay.attackingDefendingImage.sprite = attackAndDefendSprite;
        }
        else
        {
            cardDisplay.attackingDefendingImage.sprite = attackSprite;
        }
    }
    public void setIsDefending()
    {
        card.isDefending = true;
        cardDisplay.attackingDefendingImage.enabled = true;
        if (card.canAttack)
        {
            cardDisplay.attackingDefendingImage.sprite = attackAndDefendSprite;
        }
        else
        {
            cardDisplay.attackingDefendingImage.sprite = shieldSprite;
        }
    }
    public void setIsNotDefending()
    {
        card.isDefending = false;
        if (card.canAttack)
        {
            cardDisplay.attackingDefendingImage.sprite = attackSprite;
        }
        else
        {
            cardDisplay.attackingDefendingImage.enabled = false;
        }
    }
    public void setShieldBroken()
    {
        card.isDefending = false;
        cardDisplay.attackingDefendingImage.sprite = brokenShieldSprite;
    }
}
