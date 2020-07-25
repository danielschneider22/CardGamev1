using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscardManager : MonoBehaviour
{
    public List<MovingCard> movingCards;
    public GameObject discardArea;
    public GameObject discardPlacementGrid;
    public GameObject placeholderObj;
    public float cellXSize;

    private float centerOfDiscardArea;
    private float minMoveSpeed = 100f;

    public TextMeshProUGUI playerDiscardText;
    public void Start()
    {
        movingCards = new List<MovingCard>();
        centerOfDiscardArea = discardArea.transform.GetComponent<RectTransform>().rect.center.x;
    }

    private void FixedUpdate()
    {
        List<MovingCard> cardsToRemove = new List<MovingCard>();
        foreach (MovingCard movingCard in movingCards)
        {
            bool moveOccurred = moveTowardEndPoint(movingCard);
            bool resizeOccurred = rescale(movingCard);

            if (!moveOccurred && !resizeOccurred)
            {
                cardsToRemove.Add(movingCard);
            }

        }

        foreach (MovingCard movingCard in cardsToRemove)
        {
            movingCards.Remove(movingCard);
        }
        if(playerDiscardText.text != discardArea.transform.childCount.ToString())
        {
            playerDiscardText.text = discardArea.transform.childCount.ToString();
        }
    }
    public void clearMovingCards()
    {
        movingCards.Clear();
    }

    public void addCardToDiscardArea(GameObject card)
    {
        // Vector3 position = card.transform.position;
        // Vector3 scale = card.transform.localScale;
        card.transform.SetParent(discardArea.transform, false);
        card.transform.SetAsFirstSibling();
        // card.transform.position = position;
        // card.transform.localScale = scale;

        GameObject placeholder = Instantiate(placeholderObj);
        placeholder.transform.SetParent(discardPlacementGrid.transform, false);
        placeholder.transform.SetAsFirstSibling();

        resetDiscardAreaPositions();
    }

    public void resetDiscardAreaPositions(float resetSpeed = -1f)
    {
        List<MovingCard> oldMoveCards = new List<MovingCard>(movingCards);

        movingCards.Clear();
        int childCount = discardArea.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform cardTransform = discardArea.transform.GetChild(i);
            Transform placeholderTransform = discardPlacementGrid.transform.GetChild(i);

            RectTransform placeholderRectTransform = placeholderTransform.GetComponent<RectTransform>();
            RectTransform handRectTransform = cardTransform.GetComponent<RectTransform>();

            MovingCard oldMovingHandCard = getExistingMovingHandCard(oldMoveCards, cardTransform);

            cardTransform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            cardTransform.GetComponent<ToggleVisibility>().makeVisible();
            setCardPosition(childCount, i, placeholderRectTransform);
            float speed = getCardSpeed(oldMovingHandCard, placeholderRectTransform, handRectTransform, resetSpeed);

            MovingCard newHandCard = new MovingCard(cardTransform, speed, placeholderTransform, new Vector3(0f, 0f, 1));
            movingCards.Add(newHandCard);
        }
    }

    // return if movement occurred
    private bool moveTowardEndPoint(MovingCard movingCard)
    {
        RectTransform handCardRectTransform = movingCard.transform.gameObject.GetComponent<RectTransform>();
        RectTransform endPointRectTransform = movingCard.endpointTransform.gameObject.GetComponent<RectTransform>();

        Vector2 handCardPos = new Vector2(handCardRectTransform.anchoredPosition.x, handCardRectTransform.anchoredPosition.y);
        if (!positionsAreTheSame(handCardRectTransform, endPointRectTransform))
        {
            handCardRectTransform.anchoredPosition = Vector2.Lerp(handCardPos, endPointRectTransform.anchoredPosition, .15f);
            return true;
        } else
        {
            handCardRectTransform.anchoredPosition = endPointRectTransform.anchoredPosition;
            return false;
        }
    }

    // return if rescaling occurred
    private bool rescale(MovingCard movingCard)
    {
        if (movingCard.endpointScale != null && !scalesAreTheSame(movingCard.transform.localScale, movingCard.endpointScale))
        {
            if (movingCard.transform.localScale.x < movingCard.endpointScale.x)
            {
                movingCard.transform.localScale = new Vector3(movingCard.transform.localScale.x + .02f, movingCard.transform.localScale.y + .02f, movingCard.transform.localScale.z);
            }
            else
            {
                movingCard.transform.localScale = new Vector3(movingCard.transform.localScale.x - .02f, movingCard.transform.localScale.y - .02f, movingCard.transform.localScale.z);
            }
            return true;
        } else
        {
            movingCard.transform.localScale = movingCard.endpointScale;
            return false;
        }
    }

    private void setCardPosition(int childCount, int idx, RectTransform placeholderRectTransform)
    {
        float diffFromCenter = 0f;
        float centerElementIdx = (float)(childCount - 1) / 2;

        if (childCount % 2 == 0)
        {
            int centerElementLeftIdx = (childCount - 1) / 2;
            int centerElementRightIdx = (childCount - 1) / 2 + 1;

            if (idx < centerElementIdx)
            {
                diffFromCenter = ((float)(idx - centerElementLeftIdx) * cellXSize) - (cellXSize / 2);
            }
            else
            {
                diffFromCenter = ((float)(idx - centerElementRightIdx) * cellXSize) + (cellXSize / 2);
            }
        }
        else
        {
            diffFromCenter = (float)(idx - centerElementIdx) * cellXSize;
        }
        float yPos = System.Math.Min(System.Math.Abs((float)(idx - centerElementIdx)) * -13f, 5f);
        placeholderRectTransform.anchoredPosition = new Vector3(centerOfDiscardArea + diffFromCenter, yPos, 1f);
    }

    private float getCardSpeed(MovingCard oldMovingHandCard, RectTransform placeholderRectTransform, RectTransform handRectTransform, float resetSpeed)
    {
        Vector2 distanceToTravel = placeholderRectTransform.anchoredPosition - handRectTransform.anchoredPosition;
        if (oldMovingHandCard != null)
        {
            return oldMovingHandCard.speed;
        }
        else if (resetSpeed != -1f)
        {
            return resetSpeed;
        }
        else
        {
            float calculatedMoveSpeed = (float)System.Math.Pow(distanceToTravel.magnitude / 10, 2);
            return System.Math.Max(calculatedMoveSpeed, minMoveSpeed);
        }
    }

    public bool cardIsMoving(Transform card)
    {
        return findMovingHandCard(card) != null;
    }

    public void stopHandBlockingRaycasts()
    {
        foreach (Transform card in discardArea.transform)
        {
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    private void moveCardsLeft(int cardIdx)
    {
        int currcardIdx = cardIdx - 1;
        if (currcardIdx < 0) { return; }

        GameObject placeholderObj = discardPlacementGrid.transform.GetChild(currcardIdx).gameObject;
        GameObject card = discardArea.transform.GetChild(currcardIdx).gameObject;
        RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
        MovingCard movingHandCard = findMovingHandCard(card.transform);
        int distFromHoverCard = System.Math.Abs(cardIdx - currcardIdx);

        movingHandCard.speed = 200;
        movingHandCard.endpointTransform = placeholderObj.transform;

        // move all the cards to the left of the index to the left
        /*while (currcardIdx >= 0)
        {
            GameObject placeholderObj = discardPlacementGrid.transform.GetChild(currcardIdx).gameObject;
            GameObject card = discardArea.transform.GetChild(currcardIdx).gameObject;
            RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
            MovingCard movingHandCard = findMovingHandCard(card.transform);
            int distFromHoverCard = System.Math.Abs(cardIdx - currcardIdx);

            placeholderRectTransform.anchoredPosition = new Vector3(placeholderRectTransform.anchoredPosition.x - hoverXPosMove, placeholderRectTransform.anchoredPosition.y - 5f, 1f);
            movingHandCard.speed = 300;
            movingHandCard.endpointTransform = placeholderObj.transform;

            currcardIdx--;
        }*/
    }
    private void moveCardsRight(int cardIdx)
    {
        int currcardIdx = cardIdx + 1;
        if (currcardIdx > discardArea.transform.childCount - 1) { return; }

        GameObject placeholderObj = discardPlacementGrid.transform.GetChild(currcardIdx).gameObject;
        GameObject card = discardArea.transform.GetChild(currcardIdx).gameObject;
        RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
        MovingCard movingHandCard = findMovingHandCard(card.transform);

        movingHandCard.speed = 200;
        movingHandCard.endpointTransform = placeholderObj.transform;

        // move all the cards to the right of the index to the right
        /*while (currcardIdx <= discardArea.transform.childCount - 1)
        {
            GameObject placeholderObj = discardPlacementGrid.transform.GetChild(currcardIdx).gameObject;
            GameObject card = discardArea.transform.GetChild(currcardIdx).gameObject;
            RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
            MovingCard movingHandCard = findMovingHandCard(card.transform);

            placeholderRectTransform.anchoredPosition = new Vector3(placeholderRectTransform.anchoredPosition.x + hoverXPosMove, placeholderRectTransform.anchoredPosition.y - 5f, 1f);
            movingHandCard.speed = 300;
            movingHandCard.endpointTransform = placeholderObj.transform;

            currcardIdx++;
        }*/
    }

    private MovingCard findMovingHandCard(Transform card)
    {
        foreach (MovingCard movingHandCard in movingCards)
        {
            if (card == movingHandCard.transform)
            {
                return movingHandCard;
            }
        }
        return null;
    }

    private MovingCard getExistingMovingHandCard(List<MovingCard> moveCards, Transform transform)
    {
        foreach (MovingCard moveCard in moveCards)
        {
            if (moveCard.transform == transform)
            {
                return moveCard;
            }
        }
        return null;
    }

    private int getCardIdxInTransform(Transform parent, Transform card)
    {
        int counter = 0;
        foreach (Transform c in parent)
        {
            if (c == card)
            {
                return counter;
            }
            counter++;
        }
        return -1;
    }

    private bool positionsAreTheSame(RectTransform t1, RectTransform t2)
    {
        Vector2 pos1 = new Vector2(t1.anchoredPosition.x, t1.anchoredPosition.y);
        Vector2 pos2 = new Vector2(t2.anchoredPosition.x, t2.anchoredPosition.y);
        return System.Math.Abs(pos1.x - pos2.x) <= .002 && System.Math.Abs(pos1.y - pos2.y) <= .002;
        // return pos1.Equals(pos2);
    }
    private bool scalesAreTheSame(Vector3 t1, Vector3 t2)
    {
        return System.Math.Abs(t1.x - t2.x) < .01;
        // Vector2 scale1 = new Vector2(t1.x, t1.y);
        // Vector2 scale2 = new Vector2(t2.x, t2.y);
        // return scale1.Equals(scale2);
    }
}
