using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static NonCreatureCard;

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
    public Image energyImage;
    public Image innerFrame;
    public Image backLighting;
    public Image healthRed;
    public Image healthGreen;
    public Image healthOrange;
    public Image healthOutline;
    public Image transparentOverlay;
    public Image shield;

    public Image fireBack;
    // public Image shieldBack;

    public Sprite attackFrontSprite;
    public Sprite defenseFrontSprite;

    public GameObject healthBar;
    public GameObject nonCreatureTextGameObj;

    public TextMeshProUGUI effect1;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI nonCreatureEffect;

    public Material material;
    public Material fireBackMaterial;
    public string location;

    private void Awake()
    {
        if (card != null)
        {
            artworkImage.sprite = card.artwork;
            if (card is CreatureCard)
            {
                CreatureCard cardAsCreatureCard = (CreatureCard)card;
                attackText.text = cardAsCreatureCard.currAttack.ToString();
                defenseText.text = cardAsCreatureCard.currDefense.ToString();
                healthText.text = cardAsCreatureCard.currHealth.ToString() + "/" + cardAsCreatureCard.maxHealth.ToString();
                attackImage.material = material;
                defenseImage.material = material;
                healthRed.material = material;
                healthGreen.material = material;
                healthOrange.material = material;
                healthOutline.material = material;
                gameObject.AddComponent(typeof(AttackDefenseManager));
                gameObject.GetComponent<AttackDefenseManager>().defenseText = defenseText;
            }
            else if (card is NonCreatureCard)
            {
                NonCreatureCard cardAsNonCreatureCard = (NonCreatureCard)card;
                attackText.enabled = false;
                defenseText.enabled = false;
                healthText.enabled = false;
                attackImage.enabled = false;
                defenseImage.enabled = false;
                healthRed.enabled = false;
                healthGreen.enabled = false;
                healthOrange.enabled = false;
                healthOutline.enabled = false;
                switch (cardAsNonCreatureCard.borderColorType)
                {
                    case (BorderColorType.attack):
                        front.sprite = attackFrontSprite;
                        break;
                    case (BorderColorType.defend):
                        front.sprite = defenseFrontSprite;
                        break;
                }
                effect1.enabled = false;
                nonCreatureTextGameObj.SetActive(true);
                nonCreatureEffect.text = cardAsNonCreatureCard.text;
                foreach (Image slot in effectSlots)
                {
                    slot.enabled = false;
                }
                cardType.text = cardAsNonCreatureCard.frameText;
                healthBar.SetActive(false);
            }

            front.material = material;
            innerFrame.material = material;
            backLighting.material = material;
        }
        else
        {
            nameText.text = "EMPTY";
        }
        if (location.ToLower().Contains("hand") || location == "enemyIntent")
        {
            artworkImage.sprite = card.artwork;
            nameText.text = card.cardName;
            cardCostText.text = card.cardCost.ToString();
            energyImage.material = material;
            if (card is CreatureCard)
            {
                effect1.text = ((CreatureCard)card).effects[0].name;
            }

            foreach (Image slot in effectSlots)
            {
                slot.material = material;
            }
        }
        if (location.ToLower().Contains("field"))
        {
            artworkImage.sprite = ((CreatureCard) card).circleArtwork;
            Color fireColor = Color.red;
            fireBack.material = Instantiate(fireBackMaterial);
            fireBack.material.SetColor("_Color", fireColor);
            // shieldBack.material = Instantiate(fireBackMaterial);
            // Color shieldColor = Color.blue;
            // shieldBack.material.SetColor("_Color", shieldColor);
            // shieldBack.material.SetFloat("_DistortionAmount", .05f);
        }
    }

}