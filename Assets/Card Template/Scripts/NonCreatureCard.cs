﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NonCreatureCard")]
public class NonCreatureCard : Card
{
    public enum BorderColorType
    {
        attack = 0,
        defend = 1,
        other = 2,
    };
    public BorderColorType borderColorType;
    public enum Target{ playerCreature, enemyCreature, playerCreatureOrEnemyCreature, player, enemy }
    public Target target;

    public delegate void EnactEffect(GameObject targetGameObject, PlayerController targetController);
    public EnactEffect enactEffect;
    [System.Serializable]
    public class NonCreatureEffect
    {
        public NonCreatureEffectName effectName;
        public int effectNumber;
    }
    public List<NonCreatureEffect> effects; 
    public string text;
    public string frameText;
}
