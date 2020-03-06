using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public List<MovingHandCard> movingCards;
    public GameObject hand;
    public GameObject handPlacementGrid;
    public GameObject placeholderObj;

    public float cellXSize;

    private float centerOfHand;

    public void Start()
    {
        movingCards = new List<MovingHandCard>();
        centerOfHand = hand.transform.GetComponent<RectTransform>().rect.center.x;
    }

    private void FixedUpdate()
    {
        List<MovingHandCard> cardsToRemove = new List<MovingHandCard>();
        foreach(MovingHandCard handCard in movingCards)
        {
            RectTransform handCardRectTransform = handCard.transform.gameObject.GetComponent<RectTransform>();
            RectTransform endPointRectTransform = handCard.endpointTransform.gameObject.GetComponent<RectTransform>();

            Vector2 handCardPos = new Vector2(handCardRectTransform.anchoredPosition.x, handCardRectTransform.anchoredPosition.y);
            bool changeOccurred = false;
            if (!positionsAreTheSame(handCardRectTransform, endPointRectTransform))
            {
                handCardRectTransform.anchoredPosition = Vector2.MoveTowards(handCardPos, endPointRectTransform.anchoredPosition, handCard.speed * Time.deltaTime);
                changeOccurred = true;
            } 
            if (handCard.endpointScale != null && !scalesAreTheSame(handCard.transform.localScale, handCard.endpointScale))
            {
                handCard.transform.localScale = new Vector3(handCard.transform.localScale.x + .01f, handCard.transform.localScale.y + .01f, handCard.transform.localScale.z);
                changeOccurred = true;
            }
            if(!changeOccurred)
            {
                cardsToRemove.Add(handCard);
            }
        }

        foreach (MovingHandCard handCard in cardsToRemove)
        {
            movingCards.Remove(handCard);
        }   
    }

    public void addCardToHand(GameObject card)
    {
        card.transform.SetParent(hand.transform, false);
        GameObject placeholder = Instantiate(placeholderObj);
        placeholder.transform.SetParent(handPlacementGrid.transform, false);

        resetHandPositions();
    }

    private void resetHandPositions()
    {
        movingCards.Clear();
        int children = hand.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            Transform cardTransform = hand.transform.GetChild(i);
            Transform gridPosition = handPlacementGrid.transform.GetChild(i);

            RectTransform gridCell = gridPosition.GetComponent<RectTransform>();
            RectTransform handCell = cardTransform.GetComponent<RectTransform>();

            if(children % 2 == 0)
            {
                float centerElementIdx = (float)(children - 1) / 2;
                int centerElementLeftIdx = (children - 1) / 2;
                int centerElementRightIdx = (children - 1) / 2 + 1;
                float diffFromCenter = 0f;
                if(i < centerElementIdx)
                {
                    diffFromCenter = ((float)(i - centerElementLeftIdx) * cellXSize) - (cellXSize / 2);
                } else
                {
                    diffFromCenter = ((float)(i - centerElementLeftIdx) * cellXSize) + (cellXSize / 2);
                }
                gridCell.anchoredPosition = new Vector3(centerOfHand + diffFromCenter, 0, 1f);
            } else
            {
                int centerElementIdx = (children - 1) / 2;
                float diffFromCenter = (float)(i - centerElementIdx) * cellXSize;
                gridCell.anchoredPosition = new Vector3(centerOfHand + diffFromCenter, 0, 1f);
            }

            MovingHandCard newHandCard = new MovingHandCard(cardTransform, 500, gridPosition, new Vector3(.55f, .55f, 1));
            movingCards.Add(newHandCard);
        }
    }

    private bool positionsAreTheSame(RectTransform t1, RectTransform t2)
    {
        Vector2 pos1 = new Vector2(t1.anchoredPosition.x, t1.anchoredPosition.y);
        Vector2 pos2 = new Vector2(t2.anchoredPosition.x, t2.anchoredPosition.y);
        return System.Math.Abs(pos1.x - pos2.x) < .001;
        // return pos1.Equals(pos2);
    }
    private bool scalesAreTheSame(Vector3 t1, Vector3 t2)
    {
        return System.Math.Abs(t1.x - t2.x) < .001;
        // Vector2 scale1 = new Vector2(t1.x, t1.y);
        // Vector2 scale2 = new Vector2(t2.x, t2.y);
        // return scale1.Equals(scale2);
    }
}
