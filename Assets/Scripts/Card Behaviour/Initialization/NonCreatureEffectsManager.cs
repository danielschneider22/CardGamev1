using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NonCreatureCard;

public class NonCreatureEffectsManager
{
    static void EmptyEffect(GameObject targetGameObject, PlayerController cardOwnerController){ }
    static void MakeAttack(GameObject targetGameObject, PlayerController cardOwnerController)
    {
        AttackDefenseManager attackDefenseManager = targetGameObject.GetComponent<AttackDefenseManager>();
        attackDefenseManager.canAttack();
    }
    static void MakeDefend(GameObject targetGameObject, PlayerController cardOwnerController)
    {
        AttackDefenseManager attackDefenseManager = targetGameObject.GetComponent<AttackDefenseManager>();
        attackDefenseManager.setIsDefending();
    }
    static void IncreaseAttack(GameObject targetGameObject, PlayerController cardOwnerController)
    {
        CardDisplay cardDisplay = targetGameObject.GetComponent<CardDisplay>();
        ((CreatureCard)cardDisplay.card).currAttack = ((CreatureCard)cardDisplay.card).currAttack + 1;
        cardDisplay.attackText.text = ((CreatureCard)cardDisplay.card).currAttack.ToString();
    }
    static void IncreaseDefense(GameObject targetGameObject, PlayerController cardOwnerController)
    {
        CardDisplay cardDisplay = targetGameObject.GetComponent<CardDisplay>();
        ((CreatureCard)cardDisplay.card).currDefense = ((CreatureCard)cardDisplay.card).currDefense + 1;
        cardDisplay.defenseText.text = ((CreatureCard)cardDisplay.card).currDefense.ToString();
    }
    public EnactEffect getEnactEffect(List<NonCreatureEffect> effects)
    {
        EnactEffect combinedEffect = EmptyEffect;
        foreach(NonCreatureEffect effect in effects)
        {
            switch (effect.effectName)
            {
                case (NonCreatureEffectName.makeCardAttack):
                {
                    EnactEffect enactEffect = MakeAttack;
                    combinedEffect += enactEffect;
                    break;
                }
                case (NonCreatureEffectName.increaseAttack):
                {
                    EnactEffect enactEffect = IncreaseAttack;
                    combinedEffect += enactEffect;
                    break;

                }
                case (NonCreatureEffectName.increaseDefense):
                {
                    EnactEffect enactEffect = IncreaseDefense;
                    combinedEffect += enactEffect;
                    break;
                }
                case (NonCreatureEffectName.makeCardDefend):
                {
                    EnactEffect enactEffect = MakeDefend;
                    combinedEffect += enactEffect;
                    break;
                }
            }
        }
        return combinedEffect;
    }
}
