using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnManager : MonoBehaviour
{
    public EnemyIntentionManager enemyIntentionManager;

    public void endTurn()
    {
        enemyIntentionManager.enactEnemyIntent();
    }
}
