using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCanPlay : MonoBehaviour
{
    private Card card;
    private void Start()
    {
        NonCreatureEffectsManager nonCreatureEffectsManager = new NonCreatureEffectsManager();

        card = GetComponent<CardDisplay>().card;
        if (card is NonCreatureCard)
        {
            NonCreatureCard cardAsNonCreatureCard = (NonCreatureCard)card;

            cardAsNonCreatureCard.enactEffect = nonCreatureEffectsManager.getEnactEffect(cardAsNonCreatureCard.effects);
        }
    }
}
