using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreatureCard")]
public class CreatureCard : Card
{
    public int attack;
    public int currAttack;
    public int thisTurnAttack;

    public int defense;
    public int currDefense;
    public int thisTurnDefense;

    public int maxHealth;
    public int currHealth;

    public bool tapped;

    public Sprite circleArtwork;

    public bool canAttack;
    public bool isDefending;

    public CardSlotEffect[] effects;
}
