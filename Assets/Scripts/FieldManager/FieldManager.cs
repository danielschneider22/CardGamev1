using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    public List<MovingCard> movingCards;
    public GameObject field;
    public GameObject fieldPlacementGrid;
    public GameObject placeholderObj;
    public float cellXSize;

    private float centerOfField;
    private float minMoveSpeed = 100f;


    public void Start()
    {
        movingCards = new List<MovingCard>();
        centerOfField = field.transform.GetComponent<RectTransform>().rect.center.x;
    }

    private void FixedUpdate()
    {
        List<MovingCard> cardsToRemove = new List<MovingCard>();
        foreach (MovingCard fieldCard in movingCards)
        {
            bool moveOccurred = moveTowardEndPoint(fieldCard);
            bool resizeOccurred = rescale(fieldCard);

            if (!moveOccurred && !resizeOccurred)
            {
                cardsToRemove.Add(fieldCard);
            }
        }

        foreach (MovingCard fieldCard in cardsToRemove)
        {
            movingCards.Remove(fieldCard);
        }
    }
    public void clearMovingCards()
    {
        movingCards.Clear();
    }
    public void removeCardFromField(Transform cardTransform)
    {
        int removeCardIdx = getCardIdxInTransform(field.transform, cardTransform);
        DestroyImmediate(field.transform.GetChild(removeCardIdx).gameObject);
        DestroyImmediate(fieldPlacementGrid.transform.GetChild(removeCardIdx).gameObject);
        resetFieldPositions();
    }

    public void addCardToField(GameObject card)
    {
        card.transform.SetParent(field.transform, false);

        GameObject placeholder = Instantiate(placeholderObj);
        placeholder.transform.SetParent(fieldPlacementGrid.transform, false);
        placeholder.transform.SetAsFirstSibling();

        resetFieldPositions(.01f);
    }

    public void resetFieldPositions(float resetSpeed = -1f)
    {
        List<MovingCard> oldMoveCards = new List<MovingCard>(movingCards);

        movingCards.Clear();
        int childCount = field.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform cardTransform = field.transform.GetChild(i);
            Transform placeholderTransform = fieldPlacementGrid.transform.GetChild(i);

            RectTransform placeholderRectTransform = placeholderTransform.GetComponent<RectTransform>();
            RectTransform fieldRectTransform = cardTransform.GetComponent<RectTransform>();

            MovingCard oldMovingFieldCard = getExistingMovingCard(oldMoveCards, cardTransform);

            cardTransform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            cardTransform.GetComponent<ToggleVisibility>().makeVisible();
            setCardPosition(childCount, i, placeholderRectTransform);
            float speed = getCardSpeed(oldMovingFieldCard, placeholderRectTransform, fieldRectTransform, resetSpeed);

            MovingCard newMovingCard = new MovingCard(cardTransform, speed, placeholderTransform, new Vector3(.6f, .6f, 1));
            movingCards.Add(newMovingCard);
        }
    }

    // return if movement occurred
    private bool moveTowardEndPoint(MovingCard movingCard)
    {
        RectTransform movingCardRectTransform = movingCard.transform.gameObject.GetComponent<RectTransform>();
        RectTransform endPointRectTransform = movingCard.endpointTransform.gameObject.GetComponent<RectTransform>();

        Vector2 movingCardPos = new Vector2(movingCardRectTransform.anchoredPosition.x, movingCardRectTransform.anchoredPosition.y);
        if (!positionsAreTheSame(movingCardRectTransform, endPointRectTransform))
        {
            movingCardRectTransform.anchoredPosition = Vector2.Lerp(movingCardPos, endPointRectTransform.anchoredPosition, .1f);
            return true;
        }
        return false;
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
        }
        return false;
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
        float yPos = 0;
        placeholderRectTransform.anchoredPosition = new Vector3(centerOfField + diffFromCenter, yPos, 1f);
    }

    private float getCardSpeed(MovingCard oldMovingCard, RectTransform placeholderRectTransform, RectTransform fieldCardRectTransform, float resetSpeed)
    {
        Vector2 distanceToTravel = placeholderRectTransform.anchoredPosition - fieldCardRectTransform.anchoredPosition;
        if (oldMovingCard != null)
        {
            return oldMovingCard.speed;
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

    public void hoverCard(Transform card)
    {
        int cardIdx = getCardIdxInTransform(field.transform, card);
        if (cardIdx != -1)
        {
            //set-up moving cards list to move everything back to original positions
            resetFieldPositions();
        }
    }

    public bool cardIsMoving(Transform card)
    {
        return findMovingCard(card) != null;
    }

    public void stopFieldBlockingRaycasts()
    {
        foreach (Transform card in field.transform)
        {
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    
    private MovingCard findMovingCard(Transform card)
    {
        foreach (MovingCard movingCard in movingCards)
        {
            if (card == movingCard.transform)
            {
                return movingCard;
            }
        }
        return null;
    }

    private MovingCard getExistingMovingCard(List<MovingCard> moveCards, Transform transform)
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
