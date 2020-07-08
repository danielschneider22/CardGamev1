using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CantPlayRecolor : MonoBehaviour
{
    public CardDisplay cardDisplay;
    public Sprite noEnergyImage;
    public Sprite energyImage;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
    }

    public void Update()
    {
        GridLayoutGroup parentGroup = cardDisplay.GetComponentInParent<GridLayoutGroup>();
        Card droppingCard = cardDisplay.card;
        if (droppingCard.cardCost > playerController.currEnergy && cardDisplay.location == "hand" && cardDisplay.energyImage != noEnergyImage)
        {
            cardDisplay.front.color = Color.grey;
            cardDisplay.energyImage.sprite = noEnergyImage;
            cardDisplay.transparentOverlay.enabled = true;
        }
        else if (droppingCard.cardCost <= playerController.currEnergy && cardDisplay.location == "hand" && cardDisplay.energyImage != energyImage)
        {
            cardDisplay.front.color = Color.white;
            cardDisplay.energyImage.sprite = energyImage;
            cardDisplay.transparentOverlay.enabled = false;
        }
    }
}
