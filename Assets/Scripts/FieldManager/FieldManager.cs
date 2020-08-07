using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    private List<MovingCard> movingCards = new List<MovingCard>();
    public GameObject field;
    public GameObject fieldPlacementGrid;
    public GameObject placeholderObj;
    public float cellXSize;

    private float centerOfField;
    private float minMoveSpeed = 100f;


    public void Awake()
    {
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

    public void addCardToField(GameObject card, bool fromScreenSpace)
    {
        Vector2 newAnchoredPos = new Vector2();
        if(fromScreenSpace)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(field.GetComponent<RectTransform>(), card.transform.position, Camera.main, out newAnchoredPos);
        } else
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, card.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(field.GetComponent<RectTransform>(), screenPoint, Camera.main, out newAnchoredPos);
        }

        Vector3 scale = card.transform.localScale;

        card.transform.SetParent(field.transform, false);
        card.transform.GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
        card.transform.localScale = scale;

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

            MovingCard newMovingCard = new MovingCard(cardTransform, speed, placeholderTransform, new Vector3(.65f, .65f, 1));
            movingCards.Add(newMovingCard);
        }
    }

    public void resetFieldPositionsImmediately()
    {
        List<MovingCard> cardsToRemove = new List<MovingCard>();
        foreach (MovingCard movingCard in movingCards)
        {
            RectTransform movingCardRectTransform = movingCard.transform.gameObject.GetComponent<RectTransform>();
            RectTransform endPointRectTransform = movingCard.endpointTransform.gameObject.GetComponent<RectTransform>();

            movingCardRectTransform.anchoredPosition = endPointRectTransform.anchoredPosition;
            movingCardRectTransform.localScale = movingCard.endpointScale;
            cardsToRemove.Add(movingCard);
        }

        foreach (MovingCard fieldCard in cardsToRemove)
        {
            movingCards.Remove(fieldCard);
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
                movingCard.transform.localScale = new Vector3(movingCard.transform.localScale.x + .01f, movingCard.transform.localScale.y + .01f, movingCard.transform.localScale.z);
            }
            else
            {
                movingCard.transform.localScale = new Vector3(movingCard.transform.localScale.x - .01f, movingCard.transform.localScale.y - .01f, movingCard.transform.localScale.z);
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

    public bool fieldHasDefendingCard()
    {
        foreach (Transform child in field.transform)
        {
            CreatureCard card = (CreatureCard)child.GetComponent<CardDisplay>().card;
            if (card.isDefending)
            {
                return true;
            }
        }
        return false;
    }

    public bool toggleAllEnergized(bool energizeAll)
    {
        foreach (Transform child in field.transform)
        {
            if(energizeAll)
            {
                child.GetComponent<EnergizedManager>().energize();
            }
            else
            {
                child.GetComponent<EnergizedManager>().deenergize();
            }
            
        }
        return false;
    }

    public bool toggleAllDefending(bool allDefend)
    {
        foreach (Transform child in field.transform)
        {
            if (allDefend)
            {
                child.GetComponent<AttackDefenseManager>().setIsDefending();
            }
            else
            {
                child.GetComponent<AttackDefenseManager>().setIsNotDefending();
            }

        }
        return false;
    }

    public void resetDefenses()
    {
        foreach (Transform child in field.transform)
        {
            CardDisplay cardDisplay = child.gameObject.GetComponent<CardDisplay>();
            ((CreatureCard)cardDisplay.card).currDefense = ((CreatureCard)cardDisplay.card).defense;
            cardDisplay.defenseText.text = ((CreatureCard)cardDisplay.card).currDefense.ToString();

        }
    }

    public bool toggleAllCanAttack(bool allAttack)
    {
        foreach (Transform child in field.transform)
        {
            if (allAttack)
            {
                child.GetComponent<AttackDefenseManager>().canAttack();
            }
            else
            {
                child.GetComponent<AttackDefenseManager>().cantAttack();
            }

        }
        return false;
    }

    public List<CreatureCard> getFieldCards()
    {
        List<CreatureCard> cards = new List<CreatureCard>();
        foreach (Transform child in field.transform)
        {
            CreatureCard card = (CreatureCard)child.GetComponent<CardDisplay>().card;
            cards.Add(card);
        }
        return cards;
    }
    public GameObject getBestDefendingCreatureToAttack()
    {
        GameObject bestGameObjToAttack = null;
        foreach (Transform child in field.transform)
        {
            CreatureCard card = (CreatureCard)child.GetComponent<CardDisplay>().card;
            if(card.isDefending && bestGameObjToAttack == null)
            {
                bestGameObjToAttack = child.gameObject;
            } else if (card.isDefending)
            {
                CreatureCard bestCaseCard = (CreatureCard)bestGameObjToAttack.GetComponent<CardDisplay>().card;
                if(bestCaseCard.currDefense > card.currDefense || (bestCaseCard.currDefense == card.currDefense && bestCaseCard.currHealth > card.currHealth))
                {
                    bestGameObjToAttack = child.gameObject;
                }
            }
        }
        return bestGameObjToAttack;
    }

    public List<GameObject> getFieldGameObjects()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (Transform child in field.transform)
        {
            gameObjects.Add(child.gameObject);
        }
        return gameObjects;
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
