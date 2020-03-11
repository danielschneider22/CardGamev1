using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDestroy : MonoBehaviour
{
    private Material material;
    private Card defendingCard;
    private float dissolveAmount;
    private void Awake()
    {
        defendingCard = GetComponent<CardDisplay>().card;
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
            Destroy(gameObject);
        }
    }
}
