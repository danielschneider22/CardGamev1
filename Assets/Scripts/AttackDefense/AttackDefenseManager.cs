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
    public Sprite greyedAttack;

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
        if (card.currDefense < card.defense) {
            defenseText.color = zeroStatColor;
        }
    }

    public void cantAttack()
    {
        card.canAttack = false;
        cardDisplay.attackImage.sprite = greyedAttack;
    }
}
