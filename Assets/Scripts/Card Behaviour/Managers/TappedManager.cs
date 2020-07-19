using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TappedManager : MonoBehaviour
{
    private CreatureCard card;
    private CardDisplay cardDisplay;

    public Animator animator;

    private void Awake()
    {
        card = (CreatureCard) GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
    }

    public void tapCard()
    {
        animator.enabled = false;
        // string animationTrigger = "TapCard";
        // animator.SetTrigger(animationTrigger);
        gameObject.transform.eulerAngles = new Vector3(0, 0, -5f);
        card.isTapped = true;
    }
}
