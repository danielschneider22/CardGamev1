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
    public GameObject topOfHandArea;
    public float cellXSize;
    public HoverCopyTopCard hoverCopyTopCard;

    private float centerOfHand;
    private float minMoveSpeed = 100f;
    private float hoverXPosMove = 20f;
    private float hoverYPosMove = 0f;
    private float hoverMoveUp = 60f;
    

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
            bool moveOccurred = moveTowardEndPoint(handCard);
            bool resizeOccurred = rescale(handCard);
            bool rotationOcurred = rotate(handCard);

            if (!moveOccurred && !resizeOccurred && !rotationOcurred)
            {
                cardsToRemove.Add(handCard);
            }

            if(hoverCopyTopCard != null && handCard.transform == hoverCopyTopCard.handTransform)
            {
                setCopyTransformToHandTransform(handCard.transform);
            }
        }

        foreach (MovingHandCard handCard in cardsToRemove)
        {
            movingCards.Remove(handCard);
        }   
    }
    public void clearMovingCards()
    {
        movingCards.Clear();
    }
    public void removeCardFromHand(Transform cardTransform)
    {
        int removeCardIdx = getCardIdxInTransform(hand.transform, cardTransform);
        Destroy(hand.transform.GetChild(removeCardIdx));
        Destroy(handPlacementGrid.transform.GetChild(removeCardIdx));
        resetHandPositions();
    }
    public void clearTopCardFromMovingCards()
    {
        MovingHandCard removeMovingHandCard = findMovingHandCard(hoverCopyTopCard.handTransform);
        if (removeMovingHandCard != null)
        {
            movingCards.Remove(removeMovingHandCard);
        }
    }

    public void addCardToHand(GameObject card)
    {
        card.transform.SetParent(hand.transform, false);
        card.transform.SetAsFirstSibling();

        GameObject placeholder = Instantiate(placeholderObj);
        placeholder.transform.SetParent(handPlacementGrid.transform, false);
        placeholder.transform.SetAsFirstSibling();

        resetHandPositions();
    }

    public void resetHandPositions(float resetSpeed = -1f)
    {
        List<MovingHandCard> oldMoveCards = new List<MovingHandCard>(movingCards);

        removeAllTopOfHandObjs();
        movingCards.Clear();
        int childCount = hand.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform cardTransform = hand.transform.GetChild(i);
            Transform placeholderTransform = handPlacementGrid.transform.GetChild(i);

            RectTransform placeholderRectTransform = placeholderTransform.GetComponent<RectTransform>();
            RectTransform handRectTransform = cardTransform.GetComponent<RectTransform>();

            MovingHandCard oldMovingHandCard = getExistingMovingHandCard(oldMoveCards, cardTransform);

            cardTransform.GetComponent<ToggleVisibility>().makeVisible();
            setCardPosition(childCount, i, placeholderRectTransform);
            setCardRotation(childCount, i, placeholderRectTransform);
            float speed = getCardSpeed(oldMovingHandCard, placeholderRectTransform, handRectTransform, resetSpeed);

            MovingHandCard newHandCard = new MovingHandCard(cardTransform, speed, placeholderTransform, new Vector3(.55f, .55f, 1));
            movingCards.Add(newHandCard);
        }
    }

    private void removeAllTopOfHandObjs()
    {
        foreach(Transform child in topOfHandArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        hoverCopyTopCard = null;
    }

    // return if movement occurred
    private bool moveTowardEndPoint(MovingHandCard handCard)
    {
        RectTransform handCardRectTransform = handCard.transform.gameObject.GetComponent<RectTransform>();
        RectTransform endPointRectTransform = handCard.endpointTransform.gameObject.GetComponent<RectTransform>();

        Vector2 handCardPos = new Vector2(handCardRectTransform.anchoredPosition.x, handCardRectTransform.anchoredPosition.y);
        if (!positionsAreTheSame(handCardRectTransform, endPointRectTransform))
        {
            handCardRectTransform.anchoredPosition = Vector2.Lerp(handCardPos, endPointRectTransform.anchoredPosition, .15f);
            return true;
        }
        return false;
    }

    private void setCopyTransformToHandTransform(Transform handCardTransform)
    {
        RectTransform copyCardRectTransform = hoverCopyTopCard.copyTransform.GetComponent<RectTransform>();
        RectTransform handCardRectTransform = handCardTransform.GetComponent<RectTransform>();

        copyCardRectTransform.anchoredPosition = handCardRectTransform.anchoredPosition;
        copyCardRectTransform.localScale = handCardRectTransform.localScale;
    }

    // return if rescaling occurred
    private bool rescale(MovingHandCard handCard)
    {
        if (handCard.endpointScale != null && !scalesAreTheSame(handCard.transform.localScale, handCard.endpointScale))
        {
            if (handCard.transform.localScale.x < handCard.endpointScale.x)
            {
                handCard.transform.localScale = new Vector3(handCard.transform.localScale.x + .02f, handCard.transform.localScale.y + .02f, handCard.transform.localScale.z);
            }
            else
            {
                handCard.transform.localScale = new Vector3(handCard.transform.localScale.x - .02f, handCard.transform.localScale.y - .02f, handCard.transform.localScale.z);
            }
            return true;
        }
        return false;
    }

    // return if rotation change occurred
    private bool rotate(MovingHandCard handCard)
    {
        if (!rotationsAreTheSame(handCard.transform.eulerAngles, handCard.endpointTransform.eulerAngles))
        {
            handCard.transform.rotation = Quaternion.RotateTowards(handCard.transform.rotation, handCard.endpointTransform.rotation, .4f);
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
        float yPos = System.Math.Min(System.Math.Abs((float)(idx - centerElementIdx)) * -13f, 5f);
        placeholderRectTransform.anchoredPosition = new Vector3(centerOfHand + diffFromCenter, yPos, 1f);
    }

    private float getCardSpeed(MovingHandCard oldMovingHandCard, RectTransform placeholderRectTransform, RectTransform handRectTransform, float resetSpeed)
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

    private void setCardRotation(int childCount, int idx, Transform placeholderRectTransform)
    {
        float centerElementIdx = (float)(childCount - 1) / 2;
        placeholderRectTransform.eulerAngles = new Vector3(0, 0, (float)(idx - centerElementIdx) * -5f);
    }

    public void hoverCard(Transform card)
    {
        int cardIdx = getCardIdxInTransform(hand.transform, card);
        if(cardIdx != -1)
        {
            //set-up moving cards list to move everything back to original positions
            resetHandPositions();

            moveCardUp(cardIdx, card);
            moveCardsLeft(cardIdx);
            moveCardsRight(cardIdx);
        }
    }

    private void moveCardUp(int cardIdx, Transform card)
    {
        GameObject placeholderObj = handPlacementGrid.transform.GetChild(cardIdx).gameObject;
        RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
        MovingHandCard movingHandCard = findMovingHandCard(card);
        GameObject copyCard = Instantiate(card.gameObject, topOfHandArea.transform);
        hoverCopyTopCard = new HoverCopyTopCard(copyCard.transform, card);
        card.GetComponent<ToggleVisibility>().makeInvisible();

        placeholderRectTransform.anchoredPosition = new Vector3(placeholderRectTransform.anchoredPosition.x, hoverMoveUp, 1f);
        movingHandCard.speed = 600;
        movingHandCard.endpointTransform = placeholderObj.transform;
        movingHandCard.endpointScale = new Vector3(.6f, 6f, 1);
    }

    private void moveCardsLeft(int cardIdx)
    {
        int currcardIdx = cardIdx - 1;
        if(currcardIdx < 0) { return; }

        GameObject placeholderObj = handPlacementGrid.transform.GetChild(currcardIdx).gameObject;
        GameObject card = hand.transform.GetChild(currcardIdx).gameObject;
        RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
        MovingHandCard movingHandCard = findMovingHandCard(card.transform);
        int distFromHoverCard = System.Math.Abs(cardIdx - currcardIdx);

        placeholderRectTransform.anchoredPosition = new Vector3(placeholderRectTransform.anchoredPosition.x - hoverXPosMove, placeholderRectTransform.anchoredPosition.y - hoverYPosMove, 1f);
        movingHandCard.speed = 200;
        movingHandCard.endpointTransform = placeholderObj.transform;

        // move all the cards to the left of the index to the left
        /*while (currcardIdx >= 0)
        {
            GameObject placeholderObj = handPlacementGrid.transform.GetChild(currcardIdx).gameObject;
            GameObject card = hand.transform.GetChild(currcardIdx).gameObject;
            RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
            MovingHandCard movingHandCard = findMovingHandCard(card.transform);
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
        if (currcardIdx > hand.transform.childCount - 1) { return; }

        GameObject placeholderObj = handPlacementGrid.transform.GetChild(currcardIdx).gameObject;
        GameObject card = hand.transform.GetChild(currcardIdx).gameObject;
        RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
        MovingHandCard movingHandCard = findMovingHandCard(card.transform);

        placeholderRectTransform.anchoredPosition = new Vector3(placeholderRectTransform.anchoredPosition.x + hoverXPosMove, placeholderRectTransform.anchoredPosition.y - hoverYPosMove, 1f);
        movingHandCard.speed = 200;
        movingHandCard.endpointTransform = placeholderObj.transform;

        // move all the cards to the right of the index to the right
        /*while (currcardIdx <= hand.transform.childCount - 1)
        {
            GameObject placeholderObj = handPlacementGrid.transform.GetChild(currcardIdx).gameObject;
            GameObject card = hand.transform.GetChild(currcardIdx).gameObject;
            RectTransform placeholderRectTransform = placeholderObj.GetComponent<RectTransform>();
            MovingHandCard movingHandCard = findMovingHandCard(card.transform);

            placeholderRectTransform.anchoredPosition = new Vector3(placeholderRectTransform.anchoredPosition.x + hoverXPosMove, placeholderRectTransform.anchoredPosition.y - 5f, 1f);
            movingHandCard.speed = 300;
            movingHandCard.endpointTransform = placeholderObj.transform;

            currcardIdx++;
        }*/
    }

    private MovingHandCard findMovingHandCard(Transform card)
    {
        foreach(MovingHandCard movingHandCard in movingCards)
        {
            if(card == movingHandCard.transform)
            {
                return movingHandCard;
            }
        }
        return null;
    }

    private MovingHandCard getExistingMovingHandCard(List<MovingHandCard> moveCards, Transform transform)
    {
        foreach(MovingHandCard moveCard in moveCards)
        {
            if(moveCard.transform == transform)
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
    private bool rotationsAreTheSame(Vector3 t1, Vector3 t2)
    {
        return System.Math.Abs(t1.z - t2.z) < .01;
    }
}
