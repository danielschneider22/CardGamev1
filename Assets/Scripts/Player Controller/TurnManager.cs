using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class TurnManager : MonoBehaviour
{
    public EnemyIntentionManager enemyIntentionManager;
    public string showTurnPopup = "";
    public float showPopupTimer = 0f;
    public TextMeshProUGUI turnText;
    public Image turnPopupImage;
    private static float popupTime = 2f;
    public PlayerController playerController;
    public PlayerController enemyController;
    public FieldManager playerFieldManager;
    public FieldManager enemyFieldManager;

    private void FixedUpdate()
    {
        if (showPopupTimer > 0f)
        {
            showPopupTimer -= Time.deltaTime;
        }
        if(showPopupTimer <= 0f && showTurnPopup == "enemy" )
        {
            enemyIntentionManager.enactEnemyIntent();
            showTurnPopup = "";
            hideTurnPopup();
        } else if (showPopupTimer <= 0f && showTurnPopup == "player")
        {
            showTurnPopup = "";
            hideTurnPopup();
        }
    }

    public void hideTurnPopup()
    {
        turnPopupImage.enabled = false;
        turnText.enabled = false;
    }

    public void startPlayerTurn()
    {
        playerController.resetEnergy();
        showTurnPopup = "player";
        showPopupTimer = popupTime;
        turnPopupImage.enabled = true;
        turnText.enabled = true;
        turnText.text = "Player Turn";
        playerController.defendingStatus = DefendingStatus.notDefended;
        playerFieldManager.toggleAllDefending(false);
        enemyFieldManager.toggleAllEnergized(false);
        playerFieldManager.toggleAllEnergized(true);
        enemyFieldManager.toggleAllCanAttack(false);
    }

    public void endTurn()
    {
        showTurnPopup = "enemy";
        showPopupTimer = popupTime;
        turnPopupImage.enabled = true;
        turnText.enabled = true;
        turnText.text = "Enemy Turn";
        enemyController.defendingStatus = DefendingStatus.notDefended;
        enemyFieldManager.toggleAllDefending(false);
        playerFieldManager.toggleAllEnergized(false);
        enemyFieldManager.toggleAllEnergized(true);
        playerFieldManager.toggleAllCanAttack(false);

    }
}
