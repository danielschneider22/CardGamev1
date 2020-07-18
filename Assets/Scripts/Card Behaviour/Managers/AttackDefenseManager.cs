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
    public Animator animator;
    public int defenseBeforeChange;
    public bool isDefendingBeforeChange;

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
            setIsNotDefending();
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
        } else
        {
            setIsNotDefending();
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
        cardDisplay.fireBack.enabled = false;
    }
    public void canAttack()
    {
        card.canAttack = true;
        cardDisplay.fireBack.enabled = true;
    }
    public void setIsDefending()
    {
        card.isDefending = true;
        cardDisplay.shield.enabled = true;
    }
    public void setIsNotDefending()
    {
        card.isDefending = false;
        cardDisplay.shield.enabled = false;
    }
}
