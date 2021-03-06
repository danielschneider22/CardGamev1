﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CardDisplay;

public class Initialization : MonoBehaviour
{
    public GameObject card;
    public GameObject creatureCardTemplate;
    public GridLayoutGroup playerHand;
    public GridLayoutGroup enemyField;
    public Canvas screenSpaceOverlayCanvas;
    public Canvas worldCanvas;
    public int numEnemyFieldCards;
    public int numPlayerHandCards;
    public Card ninjaCard;
    public Card goblinCard;
    public Card bashCard;
    public Card defendCard;
    public Material material;
    public Material fireBackMaterial;
    public PlayerDeckManager playerDeckManager;
    public EnemyIntentionManager enemyIntentionManager;
    public int numCardsToDraw;
    public int numIntentCards = 3;
    public Transform enemyIntentArea;
    public FieldManager playerFieldManager;
    public FieldManager enemyFieldManager;
    public int numPlayerFieldCards;
    public TurnManager turnManager;

    private float enemyIntentStartTimer;
    private bool enemyTurnStarted;
    void Awake()
    {
        // initializeHand();
        // drawCards();
        // initializeEnemyField();
        // initializeEnemyIntent();
        // initializePlayerField();
        enemyIntentStartTimer = 3f;
        enemyTurnStarted = false;
        // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }

    private void Start()
    {
        turnManager.startPlayerTurn();
    }

    private void initializeHand()
    {
        for (var i = 0; i < numPlayerHandCards; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            GameObject copyCard = Instantiate(card, playerHand.transform, false);
            copyCard.GetComponent<CardDisplay>().card = copyNinja;
            copyCard.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
            copyCard.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
            copyCard.GetComponent<CardDisplay>().material = Instantiate(material);
            copyCard.GetComponent<CardDisplay>().fireBackMaterial = Instantiate(fireBackMaterial);
            copyCard.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
            copyCard.GetComponent<RectTransform>().pivot = new Vector2(.5f, .5f);
            copyCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
            copyCard.transform.SetParent(playerHand.transform, true);
            copyCard.GetComponent<Animator>().enabled = false;
        }
    }

    private void initializePlayerField()
    {
        card.SetActive(false);
        for (var i = 0; i < numPlayerFieldCards; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            creatureCardTemplate.SetActive(false);
            GameObject copyCard = Instantiate(creatureCardTemplate);
            copyCard.GetComponent<CardDisplay>().card = copyNinja;
            copyCard.GetComponent<CardDisplay>().material = Instantiate(material);
            copyCard.GetComponent<CardDisplay>().location = Location.field;
            copyCard.GetComponent<DragDropCard>().canvas = worldCanvas;
            copyCard.GetComponent<OnHover>().canvas = worldCanvas;
            copyCard.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
            copyCard.GetComponent<Animator>().enabled = false;
            copyCard.SetActive(true);
            creatureCardTemplate.SetActive(true);

            playerFieldManager.addCardToField(copyCard, true);
        }
        card.SetActive(true);
        playerFieldManager.resetFieldPositions();
    }

    private void initializeEnemyField()
    {
        card.SetActive(false);
        for (var i = 0; i < numEnemyFieldCards; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            creatureCardTemplate.SetActive(false);
            GameObject copyCard = Instantiate(creatureCardTemplate);
            copyCard.GetComponent<CardDisplay>().card = copyNinja;
            copyCard.GetComponent<CardDisplay>().material = Instantiate(material);
            copyCard.GetComponent<CardDisplay>().location = Location.enemyField;
            copyCard.GetComponent<DragDropCard>().canvas = worldCanvas;
            copyCard.GetComponent<OnHover>().canvas = worldCanvas;
            copyCard.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();
            copyCard.GetComponent<Animator>().enabled = false;
            copyCard.SetActive(true);
            if(i % 2 == 1)
            {
                copyCard.GetComponent<AttackDefenseManager>().setIsDefending();
            }
            creatureCardTemplate.SetActive(true);

            enemyFieldManager.addCardToField(copyCard, true);
        }
        card.SetActive(true);
        enemyFieldManager.resetFieldPositionsImmediately();
    }

    /* private void initializeEnemyField()
    {
        card.SetActive(false);
        for (var i = 0; i < numEnemyFieldCards; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            creatureCardTemplate.SetActive(false);
            GameObject copyCard = Instantiate(creatureCardTemplate);
            copyCard.GetComponent<CardDisplay>().card = copyNinja;
            copyCard.GetComponent<CardDisplay>().material = Instantiate(material);
            copyCard.GetComponent<CardDisplay>().location = "enemyField";
            copyCard.GetComponent<DragDropCard>().canvas = worldCanvas;
            copyCard.GetComponent<OnHover>().canvas = worldCanvas;
            copyCard.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();
            copyCard.GetComponent<Animator>().enabled = false;
            copyCard.SetActive(true);
            creatureCardTemplate.SetActive(true);

            copyCard.transform.SetParent(enemyField.transform, false);
            // var rotationVector = copyCard.GetComponent<CardDisplay>().healthBar.GetComponent<RectTransform>().rotation.eulerAngles;
            // rotationVector.x = -.2f;
            // copyCard.GetComponent<CardDisplay>().healthBar.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotationVector);
        }
        card.SetActive(true);
    } */

    public void initializeEnemyIntent()
    {
        card.SetActive(false);
        int numCards = Random.Range(2, 3);
        List<Card> enemyIntentCards = new List<Card>();
        for (var i = 0; i < numCards; i++)
        {
            int randomCard = Random.Range(0, 7);
            switch (randomCard)
            {

                case 0:
                    enemyIntentCards.Add(Instantiate(defendCard));
                    break;
                case 1:
                    enemyIntentCards.Add(Instantiate(goblinCard));
                    break;
                case 2:
                case 3:
                case 4:
                    enemyIntentCards.Add(Instantiate(ninjaCard));
                    break;
                case 5:
                case 6:
                    enemyIntentCards.Add(Instantiate(bashCard));
                    break;
            }
        }
        enemyIntentCards.Add(Instantiate(bashCard));
        enemyIntentCards.Add(Instantiate(defendCard));
        foreach (Card cardObjToCopy in enemyIntentCards)
        { 
            GameObject newCard = Instantiate(card);
            newCard.GetComponent<CardDisplay>().material = Instantiate(material);
            newCard.GetComponent<CardDisplay>().card = cardObjToCopy;
            newCard.GetComponent<CardDisplay>().location = Location.enemyIntent;
            newCard.GetComponent<DragDropCard>().canvas = worldCanvas;
            newCard.GetComponent<OnHover>().canvas = worldCanvas;
            newCard.transform.SetParent(enemyIntentArea, false);
            newCard.transform.localScale = new Vector3(.25f, .25f, .25f);
            newCard.SetActive(true);
            enemyIntentionManager.addCard(newCard);
        }
        card.SetActive(true);
    }

    private void drawCards()
    {
        playerDeckManager.drawCard();
        playerDeckManager.drawCard();
        playerDeckManager.drawCard();
        playerDeckManager.drawCard();
        playerDeckManager.drawCard();
        playerDeckManager.drawCard();
    }
}
