using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;
using static NonCreatureCard;

public class CanPlayManager
{
    static bool EmptyCanPlay(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController) { return canPlayTracker; }
    static bool PayEnergyCost(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        canPlayTracker = canPlayTracker && playerController.currEnergy >= cardBeingPlayed.cardCost;
        return canPlayTracker;
    }
    static bool CanPlayNonCreature(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        string parentObjName = targetGameObject.transform.parent.name;
        canPlayTracker = canPlayTracker && parentObjName == "Player Field";
        return canPlayTracker;
    }
    static bool CreatureIsntAttacking(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        if(!(targetCardDisplay.card is CreatureCard))
        {
            canPlayTracker = false;
            return canPlayTracker;
        }
        CreatureCard targetCardAsCreature = (CreatureCard)targetCardDisplay.card;
        canPlayTracker = canPlayTracker && targetCardAsCreature.canAttack == false;
        return canPlayTracker;
    }
    static bool CreatureIsntTapped(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        if (!(targetCardDisplay.card is CreatureCard))
        {
            canPlayTracker = false;
            return canPlayTracker;
        }
        CreatureCard targetCardAsCreature = (CreatureCard)targetCardDisplay.card;
        canPlayTracker = canPlayTracker && targetCardAsCreature.isTapped == false;
        return canPlayTracker;
    }
    static bool CreatureIsntDefending(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        if (!(targetCardDisplay.card is CreatureCard))
        {
            canPlayTracker = false;
        }
        CreatureCard targetCardAsCreature = (CreatureCard)targetCardDisplay.card;
        canPlayTracker = canPlayTracker && targetCardAsCreature.isDefending == false;
        return canPlayTracker;
    }

    public CanPlay getCanPlay(List<CanPlayRequirement> canPlayRequirements)
    {
        CanPlay combinedCanPlay = EmptyCanPlay;
        foreach(CanPlayRequirement requirement in canPlayRequirements)
        {
            switch (requirement.requirementName)
            {
                case (CanPlayRequirementName.payEnergyCost):
                {
                    CanPlay canPlay = PayEnergyCost;
                    combinedCanPlay += canPlay;
                    break;
                }
                case (CanPlayRequirementName.canPlayNonCreature):
                {
                        CanPlay canPlay = CanPlayNonCreature;
                    combinedCanPlay += canPlay;
                    break;
                }
                case (CanPlayRequirementName.creatureIsntAttacking):
                {
                    CanPlay canPlay = CreatureIsntAttacking;
                    combinedCanPlay += canPlay;
                    break;
                }
                case (CanPlayRequirementName.creatureIsntDefending):
                {
                    CanPlay canPlay = CreatureIsntDefending;
                    combinedCanPlay += canPlay;
                    break;
                }
            }
        }
        return combinedCanPlay;
    }
}
