using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CardDisplay;

public class PlayerDeckManager : MonoBehaviour
{
    public GameObject card;
    public Canvas screenSpaceOverlayCanvas;
    public Card ninjaCard;
    public Card goblinCard;
    public Card bashCard;
    public Card shortShieldCard;
    public Material material;
    public HandManager handManager;

    private List<Card> cards;
    public TextMeshProUGUI playerDeckText;
    void Start()
    {
        cards = new List<Card>();

        for (var i = 0; i < 40; i++)
        {
            if(i % 3 == 0)
            {
                cards.Add(Instantiate(goblinCard));
            } else if (i % 3 == 1)
            {
                cards.Add(Instantiate(bashCard));
            }
            else if (i % 3 == 2)
            {
                cards.Add(Instantiate(shortShieldCard));
            }

        }
        playerDeckText.text = cards.Count.ToString();
    }

    private GameObject createCopyCard(Card cardObjToCopy)
    {
        card.SetActive(false);
        GameObject newCard = Instantiate(card);
        // newCard.GetComponent<CardDisplay>().material = Instantiate(material);
        newCard.GetComponent<CardDisplay>().card = cardObjToCopy;
        newCard.GetComponent<CardDisplay>().location = Location.hand;
        newCard.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
        newCard.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
        newCard.transform.localScale = new Vector3(.01f, .01f, 1.0f);
        newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, 100f);
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
