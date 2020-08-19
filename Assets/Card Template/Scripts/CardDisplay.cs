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
    public TextMeshProUGUI retreatCostText;

    public Image artworkImage;

    public Image front;
    [SerializeField]
    public List<Image> effectSlots;
    public Image attackImage;
    public Image retreatImage;
    public Image retreatImageBack;
    public Image defenseImage;
    public Image energyImage;
    public Image innerFrame;
    public Image backLighting;
    public Image healthRed;
    public Image healthGreen;
    public Image healthOrange;
    public Image healthOutline;
    public Image transparentOverlay;
    public Image attackingDefendingImage;

    public Image fireBack;
    // public Image shieldBack;

    public Sprite attackFrontSprite;
    public Sprite defenseFrontSprite;

    public GameObject healthBar;
    public GameObject nonCreatureTextGameObj;

    [SerializeField]
    public List<Image> effectIconImages;
    [SerializeField]
    public List<TextMeshProUGUI> effectNames;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI nonCreatureEffect;

    public Material material;
    public Material fireBackMaterial;
    public enum Location { field, enemyField, hand, enemyIntent, discard }
    public Location location;

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
                retreatCostText.text = cardAsCreatureCard.retreatCost.ToString();
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
                retreatImage.enabled = false;
                retreatImageBack.enabled = false;
                retreatCostText.enabled = false;
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
                foreach (Image effectIcon in effectIconImages)
                {
                    effectIcon.enabled = false;
                }
                foreach (TextMeshProUGUI effectName in effectNames)
                {
                    effectName.gameObject.SetActive(false);
                }
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
        if (location == Location.hand || location == Location.enemyIntent)
        {
            artworkImage.sprite = card.artwork;
            nameText.text = card.cardName;
            cardCostText.text = card.cardCost.ToString();
            energyImage.material = material;
            if (card is CreatureCard)
            {
                int i = 0;
                foreach (Image effectIcon in effectIconImages)
                {
                    effectIcon.enabled = false;
                }
                foreach (TextMeshProUGUI effectName in effectNames)
                {
                    effectName.gameObject.SetActive(false);
                }
                foreach (CardSlotEffect effect in ((CreatureCard)card).effects) {
                    effectNames[i].text = ((CreatureCard)card).effects[i].description;
                    effectNames[i].gameObject.SetActive(true);
                    effectIconImages[i].sprite = ((CreatureCard)card).effects[i].icon;
                    effectIconImages[i].enabled = true;
                    i = i + 1;
                }
            }

            foreach (Image slot in effectSlots)
            {
                slot.material = material;
            }
        }
        if (location == Location.field || location == Location.enemyField)
        {
            artworkImage.sprite = ((CreatureCard) card).circleArtwork;
            Color fireColor = Color.red;
            fireBack.material = Instantiate(fireBackMaterial);
            fireBack.material.SetColor("_Color", fireColor);
            foreach (Image effectIcon in effectIconImages)
            {
                effectIcon.enabled = false;
            }
            int i = 0;
            foreach (CardSlotEffect effect in ((CreatureCard)card).effects)
            {
                effectIconImages[i].sprite = ((CreatureCard)card).effects[i].icon;
                effectIconImages[i].enabled = true;
                i = i + 1;
            }
        }
    }

}