using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string cardType;

    public CardSlotEffect[] effects;

    public bool isDestroyed;

    public Sprite artwork;

    public int cardCost;
}
