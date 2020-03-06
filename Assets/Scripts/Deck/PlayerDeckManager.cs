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
        Card copyNinja = Instantiate(ninjaCard);
        Card copyGoblin = Instantiate(goblinCard);

        cards = new List<Card>();

        for (var i = 0; i < 10; i++)
        {
            cards.Add(Instantiate(copyNinja));
        }
    }

    private GameObject createCopyCard(Card cardObjToCopy)
    {
        GameObject copyCard = Instantiate(card);
        copyCard.GetComponent<CardDisplay>().card = cardObjToCopy;
        copyCard.GetComponent<DragDropCard>().canvas = screenSpaceOverlayCanvas;
        copyCard.GetComponent<OnHover>().canvas = screenSpaceOverlayCanvas;
        copyCard.GetComponent<CardDisplay>().material = Instantiate(material);
        copyCard.GetComponent<ChangeBackgroundLighting>().selectableBacklighting();
        copyCard.GetComponent<RectTransform>().pivot = new Vector2(.5f, .5f);
        copyCard.GetComponent<Animator>().enabled = false;
        GameObject newCard = Instantiate(card);
        newCard.transform.localScale = new Vector3(.01f, .01f, 1.0f);
        newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, 30f);
        // newCard.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        // newCard.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        return newCard;
    }
    public void drawCard()
    {
        handManager.addCardToHand(createCopyCard(cards[0]));
        cards.Remove(cards[0]);
    }
}
