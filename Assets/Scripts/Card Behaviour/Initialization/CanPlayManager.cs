using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;
using static NonCreatureCard;

public class CanPlayManager
{
    static bool EmptyCanPlay(GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController) { return true; }
    static bool PayEnergyCost(GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        return playerController.currEnergy >= cardBeingPlayed.cardCost;
    }
    static bool CanPlayNonCreature(GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        string parentObjName = targetGameObject.transform.parent.name;
        return parentObjName == "Player Field";
    }
    static bool CreatureIsntAttacking(GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        if(!(targetCardDisplay.card is CreatureCard))
        {
            return false;
        }
        CreatureCard targetCardAsCreature = (CreatureCard)targetCardDisplay.card;
        return targetCardAsCreature.canAttack == false;
    }
    static bool CreatureIsEnergized(GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        if (!(targetCardDisplay.card is CreatureCard))
        {
            return false;
        }
        CreatureCard targetCardAsCreature = (CreatureCard)targetCardDisplay.card;
        return targetCardAsCreature.energized == true;
    }
    static bool CreatureIsntDefending(GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController)
    {
        CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
        if (!(targetCardDisplay.card is CreatureCard))
        {
            return false;
        }
        CreatureCard targetCardAsCreature = (CreatureCard)targetCardDisplay.card;
        return targetCardAsCreature.isDefending == false;
    }

    public static bool canPlay(
        List<CanPlayRequirement> canPlayRequirements,
        GameObject targetGameObject,
        Card cardBeingPlayed,
        PlayerController playerController
    )
    {
        bool canPlay = true;
        foreach(CanPlayRequirement requirement in canPlayRequirements)
        {
            if(!canPlay) { return false; }
            switch (requirement.requirementName)
            {
                case (CanPlayRequirementName.payEnergyCost):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.canPlayNonCreature):
                {
                    canPlay = canPlay && CanPlayNonCreature(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.creatureIsntAttacking):
                {
                    canPlay = canPlay && CreatureIsntAttacking(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.creatureIsntDefending):
                {
                    canPlay = canPlay && CreatureIsntDefending(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.creatureIsEnergized):
                {
                    canPlay = canPlay && CreatureIsEnergized(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.genericCanPlayCreature):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.genericCanPlayAttackCard):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, playerController);
                    canPlay = canPlay && CanPlayNonCreature(targetGameObject, cardBeingPlayed, playerController);
                    canPlay = canPlay && CreatureIsntAttacking(targetGameObject, cardBeingPlayed, playerController);
                    canPlay = canPlay && CreatureIsEnergized(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
                case (CanPlayRequirementName.genericCanPlayDefendCard):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, playerController);
                    canPlay = canPlay && CanPlayNonCreature(targetGameObject, cardBeingPlayed, playerController);
                    canPlay = canPlay && CreatureIsntDefending(targetGameObject, cardBeingPlayed, playerController);
                    break;
                }
            }
        }
        return canPlay;
    }
}
