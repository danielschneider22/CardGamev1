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
            handCard.transform.position = Vector2.MoveTowards(handCardPos, handCard.endpointTransform.position, handCard.speed * Time.deltaTime);
        }
    }

    public void addCardToHand(GameObject card, float speed)
    {
        HandCard newHandCard = new HandCard(card.transform, speed, testTransform);
        handCards.Add(newHandCard);
    }
}
