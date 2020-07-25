using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class TurnManager : MonoBehaviour
{
    public EnemyIntentionManager enemyIntentionManager;
    private string showTurnPopup = "";
    private float showPopupTimer = 0f;
    public TextMeshProUGUI turnText;
    public Image turnPopupImage;
    private static float popupTime = 2f;
    public PlayerController playerController;
    public PlayerController enemyController;
    public FieldManager playerFieldManager;
    public FieldManager enemyFieldManager;
    public HandManager playerHandManager;
    public PlayerDeckManager playerDeckManager;
    public Initialization initializationManager;

    private bool discardHand;
    private float discardTimer = 0f;

    private bool drawToHandSize;
    private float drawTimer = 0f;
    private int numCardsDrawn = 0;
    public int handSize;

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

        if(discardHand)
        {
            List<GameObject> handCards = playerHandManager.getCards();
            if(discardTimer <= 0f && handCards.Count > 0)
            {
                playerHandManager.discardCard(handCards[0]);
                discardTimer = .1f;
            } else if (handCards.Count == 0)
            {
                discardHand = false;
                showEnemyTurnPopup();
                resetEnergiesAndStatusesForEnemyTurn();
            } else
            {
                discardTimer -= Time.deltaTime;
            }
        }

        if (drawToHandSize)
        {
            if (drawTimer <= 0f && numCardsDrawn < handSize)
            {
                playerDeckManager.drawCard();
                drawTimer = .3f;
                numCardsDrawn = numCardsDrawn + 1;
            }
            else if (numCardsDrawn == handSize)
            {
                drawToHandSize = false;
                numCardsDrawn = 0;
            }
            else
            {
                drawTimer -= Time.deltaTime;
            }
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
        playerFieldManager.resetDefenses();
        enemyFieldManager.toggleAllEnergized(false);
        playerFieldManager.toggleAllEnergized(true);
        enemyFieldManager.toggleAllCanAttack(false);
        drawToHandSize = true;
        initializationManager.initializeEnemyIntent();
    }

    public void endTurn()
    {
        discardHand = true;
    }
    private void showEnemyTurnPopup()
    {
        showTurnPopup = "enemy";
        showPopupTimer = popupTime;
        turnPopupImage.enabled = true;
        turnText.enabled = true;
        turnText.text = "Enemy Turn";
    }

    private void resetEnergiesAndStatusesForEnemyTurn()
    {
        enemyController.resetEnergy();
        enemyController.defendingStatus = DefendingStatus.notDefended;
        enemyFieldManager.toggleAllDefending(false);
        playerFieldManager.toggleAllEnergized(false);
        enemyFieldManager.toggleAllEnergized(true);
        playerFieldManager.toggleAllCanAttack(false);
        enemyFieldManager.resetDefenses();
    }
}
