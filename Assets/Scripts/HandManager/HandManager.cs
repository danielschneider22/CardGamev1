using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<HandCard> handCards;
    public Transform testTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        foreach(HandCard handCard in handCards)
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
                handCards.Remove(handCard);
            }
        }
    }

    public void addCardToHand(GameObject card, float speed)
    {
        HandCard newHandCard = new HandCard(card.transform, speed, testTransform, new Vector3(.2f, .2f, 1));
        handCards.Add(newHandCard);
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
