using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreatureCard")]
public class CreatureCard : Card
{
    public int attack;
    public int currAttack;

    public int defense;
    public int currDefense;

    public int maxHealth;
    public int currHealth;

    public Sprite circleArtwork;

    public bool canAttack;
    public bool isDefending;
}
