using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergizedManager : MonoBehaviour
{
    private CreatureCard card;
    private CardDisplay cardDisplay;

    public Animator animator;

    private void Awake()
    {
        card = (CreatureCard) GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
    }

    public void deenergize()
    {
        card.energized = false;
        cardDisplay.fireBack.enabled = false;
    }

    public void energize()
    {
        card.energized = true;
        cardDisplay.fireBack.enabled = true;
    }
}
