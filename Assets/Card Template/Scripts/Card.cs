﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string cardType;

    public CardSlotEffect[] effects;

    public Sprite artwork;

    public int attack;
    public int defense;
    public int maxHealth;
    public int currHealth;
    public int cardCost;
}
