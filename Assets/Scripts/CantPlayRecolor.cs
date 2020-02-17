using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CantPlayRecolor : MonoBehaviour
{
    public CardDisplay cardDisplay;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player Controller").GetComponent<PlayerController>();
    }

    public void Update()
    {
        Card droppingCard = cardDisplay.card;
        if (droppingCard.cardCost > playerController.currEnergy)
        {
            cardDisplay.cardCostText.color = Color.red;
        }
    }
}
