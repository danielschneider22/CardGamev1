﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NonCreatureCard;

public class NonCreatureEffectsManager
{
    static void EmptyEffect(GameObject targetGameObject){ }
    static void MakeAttack(GameObject targetGameObject)
    {
        AttackDefenseManager attackDefenseManager = targetGameObject.GetComponent<AttackDefenseManager>();
        attackDefenseManager.canAttack();
    }
    static void MakeDefend(GameObject targetGameObject)
    {
        AttackDefenseManager attackDefenseManager = targetGameObject.GetComponent<AttackDefenseManager>();
        attackDefenseManager.setIsDefending();
    }
    static void IncreaseAttack(GameObject targetGameObject)
    {
        CardDisplay cardDisplay = targetGameObject.GetComponent<CardDisplay>();
        ((CreatureCard)cardDisplay.card).currAttack = ((CreatureCard)cardDisplay.card).currAttack + 1;
        cardDisplay.attackText.text = ((CreatureCard)cardDisplay.card).currAttack.ToString();
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