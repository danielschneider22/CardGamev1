﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeEffects : MonoBehaviour
{
    private Card card;
    private void Start()
    {
        CanPlayManager canPlayManager = new CanPlayManager();

        card = GetComponent<CardDisplay>().card;
        if (card is NonCreatureCard)
        {
            card.canPlay = canPlayManager.getCanPlay(card.canPlayRequirements);
        }
    }
}
