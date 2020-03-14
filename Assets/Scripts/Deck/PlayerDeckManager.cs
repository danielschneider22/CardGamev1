using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI playerDeckText;
    void Start()
    {
        cards = new List<Card>();

        for (var i = 0; i < 10; i++)
        {
            Card copyNinja = Instantiate(ninjaCard);
            Card copyGoblin = Instantiate(goblinCard);
            cards.Add(Instantiate(goblinCard));
        }
        playerDeckText.text = cards.Count.ToString();
    }

    private GameObject createCopyCard(Card cardObjToCopy)
    {
        // card.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
        // card.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
        card.SetActive(false);
        GameObject newCard = Instantiate(card);
        newCard.GetComponent<CardDisplay>().material = Instantiate(material);
        newCard.GetComponent<CardDisplay>().card = cardObjToCopy;
        newCard.GetComponent<CardDisplay>().location = "hand";
        newCard.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
        newCard.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
        newCard.transform.localScale = new Vector3(.01f, .01f, 1.0f);
        newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, 80f);
        newCard.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
        newCard.SetActive(true);
        card.SetActive(true);
        return newCard;
    }
    public void drawCard()
    {
        handManager.addCardToHand(createCopyCard(cards[0]));
        cards.Remove(cards[0]);
        playerDeckText.text = cards.Count.ToString();
    }
}
