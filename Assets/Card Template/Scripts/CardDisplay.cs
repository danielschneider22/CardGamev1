using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI cardCostText;

    public Image artworkImage;

    public TextMeshProUGUI effect1;

    private void Start()
    {
        if(card != null)
        {
            nameText.text = card.name;
            artworkImage.sprite = card.artwork;
            attackText.text = card.attack.ToString();
            defenseText.text = card.defense.ToString();
            healthText.text = card.currHealth.ToString() + "/" + card.maxHealth.ToString();
            cardCostText.text = card.cardCost.ToString();
            effect1.text = card.effects[0].name;

        } else
        {
            nameText.text = "EMPTY";
        }
        
    }
}
