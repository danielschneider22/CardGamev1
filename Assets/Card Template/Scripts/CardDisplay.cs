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

    public Image front;
    [SerializeField]
    public Image[] effectSlots;
    public Image attackImage;
    public Image defenseImage;
    public Image innerFrame;
    public Image backLighting;
    public Image healthRed;
    public Image healthGreen;
    public Image healthOrange;
    public Image healthOutline;

    public GameObject healthBar;

    public TextMeshProUGUI effect1;
    public Material material;

    private void Awake()
    {
        if(card != null)
        {
            nameText.text = card.cardName;
            artworkImage.sprite = card.artwork;
            attackText.text = card.currAttack.ToString();
            defenseText.text = card.currDefense.ToString();
            healthText.text = card.currHealth.ToString() + "/" + card.maxHealth.ToString();
            cardCostText.text = card.cardCost.ToString();
            effect1.text = card.effects[0].name;

            front.material = material;
            attackImage.material = material;
            defenseImage.material = material;
            innerFrame.material = material;
            backLighting.material = material;
            healthRed.material = material;
            healthGreen.material = material;
            healthOrange.material = material;
            healthOutline.material = material;
            foreach (Image slot in effectSlots)
            {
                slot.material = material;
            }

        } else
        {
            nameText.text = "EMPTY";
        }
        
    }
}
