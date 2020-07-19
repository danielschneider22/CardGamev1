using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public EnemyIntentionManager enemyIntentionManager;
    public string showTurnPopup = "";
    public float showPopupTimer = 0f;
    public TextMeshProUGUI turnText;
    public Image turnPopupImage;
    private static float popupTime = 2f;
    public PlayerController playerController;

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
        showTurnPopup = "player";
        showPopupTimer = popupTime;
        turnPopupImage.enabled = true;
        turnText.enabled = true;
        turnText.text = "Player Turn";
        playerController.resetEnergy();


    }

    public void endTurn()
    {
        showTurnPopup = "enemy";
        showPopupTimer = popupTime;
        turnPopupImage.enabled = true;
        turnText.enabled = true;
        turnText.text = "Enemy Turn";

    }
}
