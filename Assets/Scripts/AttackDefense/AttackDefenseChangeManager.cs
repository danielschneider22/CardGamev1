using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackDefenseChangeManager : MonoBehaviour
{
    private CreatureCard card;
    private bool validEnterTriggered;

    public TextMeshProUGUI defenseText;

    private void Awake()
    {
        card = (CreatureCard) GetComponent<CardDisplay>().card;
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
            defenseText.color = new Color(0.6415094f, 0.2935208f, 0.2935208f);
        }
    }
}
