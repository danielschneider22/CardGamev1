using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Material material;
    public PlayerDeckManager playerDeckManager;
    public int numCardsToDraw;

    private float drawCardTimer;
    private int numCardsDrawn;
    void Awake()
    {
        // initializeHand();
        // drawCards();
        initializeEnemyField();
        drawCardTimer = 0f;
        numCardsDrawn = 0;
    }

    private void Update()
    {
        if(drawCardTimer <= 0f && numCardsDrawn < numCardsToDraw)
        {
            playerDeckManager.drawCard();
            drawCardTimer = .3f;
            numCardsDrawn++;
        } else if (numCardsDrawn < numCardsToDraw)
        {
            drawCardTimer -= Time.deltaTime;
        }
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
            copyCard.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
            copyCard.GetComponent<RectTransform>().pivot = new Vector2(.5f, .5f);
            copyCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
            copyCard.transform.SetParent(playerHand.transform, true);
            copyCard.GetComponent<Animator>().enabled = false;
        }
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
            copyCard.GetComponent<CardDisplay>().location = "enemyField";
            copyCard.GetComponent<DragDropCard>().canvas = worldCanvas;
            copyCard.GetComponent<OnHover>().canvas = worldCanvas;
            copyCard.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();
            copyCard.SetActive(true);
            creatureCardTemplate.SetActive(true);

            copyCard.transform.SetParent(enemyField.transform, false);
            // var rotationVector = copyCard.GetComponent<CardDisplay>().healthBar.GetComponent<RectTransform>().rotation.eulerAngles;
            // rotationVector.x = -.2f;
            // copyCard.GetComponent<CardDisplay>().healthBar.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotationVector);
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
