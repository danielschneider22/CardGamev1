using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initialization : MonoBehaviour
{
    public GameObject card;
    public GridLayoutGroup playerHand;
    public GridLayoutGroup enemyField;
    public Canvas screenSpaceOverlayCanvas;
    public Canvas worldCanvas;
    public int numEnemyFieldCards;
    public int numPlayerHandCards;
    public Card ninjaCard;
    public Card goblinCard;
    public Material material;
    void Start()
    {
        initializeHand();
        initializeEnemyField();
    }

    private void initializeHand()
    {
        for (var i = 0; i < numPlayerHandCards; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            card.GetComponent<CardDisplay>().card = copyNinja;
            card.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
            card.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
            card.GetComponent<CardDisplay>().material = Instantiate(material);
            card.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
            card.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            GameObject copyCard = Instantiate(card, playerHand.transform, false);
            copyCard.transform.SetParent(playerHand.transform, true);
        }
    }

    private void initializeEnemyField()
    {
        for(var i = 0; i < numEnemyFieldCards; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            card.GetComponent<CardDisplay>().card = copyNinja;
            card.GetComponent<CardDisplay>().material = Instantiate(material);
            card.GetComponent<DragDropCard>().canvas = worldCanvas;
            card.GetComponent<OnHover>().canvas = worldCanvas;
            card.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();
            GameObject copyCard = Instantiate(card);
            copyCard.transform.SetParent(enemyField.transform, false);
        }
        
    }
}
