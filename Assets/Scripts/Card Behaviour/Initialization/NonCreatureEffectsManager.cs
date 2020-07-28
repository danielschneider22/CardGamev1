using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NonCreatureCard;

public class NonCreatureEffectsManager
{
    static void EmptyEffect(int effectAmount, GameObject targetGameObject, PlayerController cardOwnerController){ }
    static void MakeAttack(int effectAmount, GameObject targetGameObject, PlayerController cardOwnerController)
    {
        AttackDefenseManager attackDefenseManager = targetGameObject.GetComponent<AttackDefenseManager>();
        attackDefenseManager.canAttack();
    }
    static void MakeDefend(int effectAmount, GameObject targetGameObject, PlayerController cardOwnerController)
    {
        AttackDefenseManager attackDefenseManager = targetGameObject.GetComponent<AttackDefenseManager>();
        attackDefenseManager.setIsDefending();
    }
    static void IncreaseAttack(int effectAmount, GameObject targetGameObject, PlayerController cardOwnerController)
    {
        CardDisplay cardDisplay = targetGameObject.GetComponent<CardDisplay>();
        ((CreatureCard)cardDisplay.card).currAttack = ((CreatureCard)cardDisplay.card).currAttack + effectAmount;
        cardDisplay.attackText.text = ((CreatureCard)cardDisplay.card).currAttack.ToString();
    }
    static void IncreaseDefense(int effectAmount, GameObject targetGameObject, PlayerController cardOwnerController)
    {
        CardDisplay cardDisplay = targetGameObject.GetComponent<CardDisplay>();
        ((CreatureCard)cardDisplay.card).currDefense = ((CreatureCard)cardDisplay.card).currDefense + effectAmount;
        cardDisplay.defenseText.text = ((CreatureCard)cardDisplay.card).currDefense.ToString();
    }
    public static void enactNonCreatureEffect(
        List<NonCreatureEffect> effects,
        GameObject targetGameObject,
        PlayerController cardOwnerController
    )
    {
        foreach(NonCreatureEffect effect in effects)
        {
            switch (effect.effectName)
            {
                case (NonCreatureEffectName.makeCardAttack):
                {
                    MakeAttack(effect.effectAmount, targetGameObject, cardOwnerController);
                    break;
                }
                case (NonCreatureEffectName.increaseAttack):
                {
                    IncreaseAttack(effect.effectAmount, targetGameObject, cardOwnerController);
                    break;

                }
                case (NonCreatureEffectName.increaseDefense):
                {
                    IncreaseDefense(effect.effectAmount, targetGameObject, cardOwnerController);
                    break;
                }
                case (NonCreatureEffectName.makeCardDefend):
                {
                    MakeDefend(effect.effectAmount, targetGameObject, cardOwnerController);
                    break;
                }
            }
        }
    }
}
