using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public List<MovingHandCard> movingCards;
    public GameObject hand;
    public GridLayoutGroup handPlacementGrid;
    public GameObject placeholderObj;

    public void Start()
    {
        movingCards = new List<MovingHandCard>();
    }

    private void FixedUpdate()
    {
        List<MovingHandCard> cardsToRemove = new List<MovingHandCard>();
        foreach(MovingHandCard handCard in movingCards)
        {
            Vector2 handCardPos = new Vector2(handCard.transform.position.x, handCard.transform.position.y);
            bool changeOccurred = false;
            if (!positionsAreTheSame(handCard.transform, handCard.endpointTransform))
            {
                handCard.transform.position = Vector2.MoveTowards(handCardPos, handCard.endpointTransform.position, handCard.speed * Time.deltaTime);
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
            MovingHandCard newHandCard = new MovingHandCard(cardTransform, 500, gridPosition, new Vector3(.55f, .55f, 1));
            movingCards.Add(newHandCard);
        }
    }

    private bool positionsAreTheSame(Transform t1, Transform t2)
    {
        Vector2 pos1 = new Vector2(t1.transform.position.x, t1.transform.position.y);
        Vector2 pos2 = new Vector2(t2.transform.position.x, t2.transform.position.y);
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
