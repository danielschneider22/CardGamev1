﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction
{
    public GameObject card;
    public GameObject cardTarget;
    public bool showingHover;
    public bool showingArrow;

    public EnemyAction(GameObject card, GameObject cardTarget, EnemyActionType actionType)
    {
        this.card = card;
        this.cardTarget = cardTarget;
        this.actionType = actionType;
    }

    public EnemyActionType actionType;
}
