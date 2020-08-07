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
        if(!playerController.name.Contains("EnemyController"))
        {
            CardDisplay targetCardDisplay = targetGameObject.GetComponent<CardDisplay>();
            string parentObjName = targetGameObject.transform.parent.name;
            return parentObjName == "Player Field";
        }
        return true;
        
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
        GameObject targetGameObject,
        Card cardBeingPlayed,
        PlayerController cardOwnerController
    )
    {
        List<CanPlayRequirement> canPlayRequirements = cardBeingPlayed.canPlayRequirements;
        bool canPlay = true;
        foreach(CanPlayRequirement requirement in canPlayRequirements)
        {
            if(!canPlay) { return false; }
            switch (requirement.requirementName)
            {
                case (CanPlayRequirementName.payEnergyCost):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.canPlayNonCreature):
                {
                    canPlay = canPlay && CanPlayNonCreature(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.creatureIsntAttacking):
                {
                    canPlay = canPlay && CreatureIsntAttacking(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.creatureIsntDefending):
                {
                    canPlay = canPlay && CreatureIsntDefending(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.creatureIsEnergized):
                {
                    canPlay = canPlay && CreatureIsEnergized(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.genericCanPlayCreature):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.genericCanPlayAttackCard):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, cardOwnerController);
                    canPlay = canPlay && CanPlayNonCreature(targetGameObject, cardBeingPlayed, cardOwnerController);
                    canPlay = canPlay && CreatureIsntAttacking(targetGameObject, cardBeingPlayed, cardOwnerController);
                    canPlay = canPlay && CreatureIsEnergized(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
                case (CanPlayRequirementName.genericCanPlayDefendCard):
                {
                    canPlay = canPlay && PayEnergyCost(targetGameObject, cardBeingPlayed, cardOwnerController);
                    canPlay = canPlay && CanPlayNonCreature(targetGameObject, cardBeingPlayed, cardOwnerController);
                    canPlay = canPlay && CreatureIsntDefending(targetGameObject, cardBeingPlayed, cardOwnerController);
                    break;
                }
            }
        }
        return canPlay;
    }
}
