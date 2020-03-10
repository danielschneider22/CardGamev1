using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeckManager : MonoBehaviour
{
    public GameObject card;
    public Canvas screenSpaceOverlayCanvas;
    public Card ninjaCard;
    public Card goblinCard;
    public Material material;
    public HandManager handManager;

    private List<Card> cards;
    void Start()
    {
        cards = new List<Card>();

        for (var i = 0; i < 10; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            cards.Add(Instantiate(copyNinja));
        }
    }

    private GameObject createCopyCard(Card cardObjToCopy)
    {
        GameObject newCard = Instantiate(card);
        newCard.transform.localScale = new Vector3(.01f, .01f, 1.0f);
        newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, 80f);
        newCard.GetComponent<CardDisplay>().material = Instantiate(material);
        newCard.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
        return newCard;
    }
    public void drawCard()
    {
        handManager.addCardToHand(createCopyCard(cards[0]));
        cards.Remove(cards[0]);
    }
}
