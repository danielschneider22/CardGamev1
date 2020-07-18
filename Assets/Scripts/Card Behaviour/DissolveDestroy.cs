using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardDisplay;

public class DissolveDestroy : MonoBehaviour
{
    private Material material;
    private Card defendingCard;
    private CardDisplay cardDisplay;
    private float dissolveAmount;
    private void Awake()
    {
        defendingCard = GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
        material = GetComponent<CardDisplay>().material;
        dissolveAmount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (defendingCard.isDestroyed && dissolveAmount < 1)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + (Time.deltaTime));
            material.SetFloat("_DissolveAmount", dissolveAmount);
        } else if (defendingCard.isDestroyed && dissolveAmount >= 1)
        {
            if(cardDisplay.location == Location.enemyField)
            {
                GameObject.FindGameObjectWithTag("Enemy Field Manager").GetComponent<FieldManager>().removeCardFromField(gameObject.transform);
            } else if (cardDisplay.location == Location.field)
            {
                GameObject.FindGameObjectWithTag("Player Field Manager").GetComponent<FieldManager>().removeCardFromField(gameObject.transform);
            }
        }
    }
}
