using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackDefenseManager : MonoBehaviour
{
    private CreatureCard card;
    private CardDisplay cardDisplay;
    private bool validEnterTriggered;
    private Color zeroStatColor = new Color(0.6415094f, 0.2935208f, 0.2935208f);

    public TextMeshProUGUI defenseText;
    public Animator animator;
    public int defenseBeforeChange;
    // public Sprite greyedAttack;

    private void Awake()
    {
        card = (CreatureCard) GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
    }
    public void decreaseDefense(int amountToDecrease)
    {
        card.currDefense = card.currDefense - amountToDecrease;
        if(card.currDefense < 0)
        {
            card.currDefense = 0;
        }
        defenseText.text = card.currDefense.ToString();
    }

    public void tempDecreaseDefense(int amountToDecrease)
    {
        defenseBeforeChange = card.currDefense;
        decreaseDefense(amountToDecrease);
    }

    public void restoreTempDefense()
    {
        card.currDefense = defenseBeforeChange;
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
        // cardDisplay.attackImage.sprite = greyedAttack;
    }
}
