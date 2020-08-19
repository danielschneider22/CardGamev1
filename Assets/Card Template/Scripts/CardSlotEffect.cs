using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSlotEffect
{
    public string name;
    [TextArea(3, 10)]
    public string description;
    public Sprite icon;
    public List<CardSlotEffectType> cardSlotEffectType;
    public enum CardSlotEffectType { enterTheBattlefield, beforeAttack, afterAttack, beforeDefending, afterDefending, beginningOfTurn, endOfTurn }
}
