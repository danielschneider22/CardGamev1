﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthFillGreen;
    public Image healthFillRed;
    public TextMeshProUGUI healthText;
    public CardDisplay cardDisplay;

    private CreatureCard card;

    public void Awake()
    {
        card = (CreatureCard)cardDisplay.card;
    }

    private float currHealthProportion;
    private int currHealth;
    public int tempDamage;
    public void tempDecreaseHealth(int damage)
    {
        tempDamage = damage;
        currHealth = card.currHealth;
        currHealthProportion = healthFillGreen.fillAmount;

        decreaseHealthText(damage);
        adjustFillBars(damage);
    }

    public void restoreTempHealth()
    {
        healthFillGreen.fillAmount = currHealthProportion;
        healthText.text = currHealth.ToString() + "/" + card.maxHealth;
    }
    
    // returns if it should be destroyed
    public bool applyTempDecreaseHealth()
    {
        healthFillRed.fillAmount = 1 - healthFillGreen.fillAmount;
        card.currHealth = Mathf.Max(card.currHealth - tempDamage, 0);
        return card.currHealth <= 0;
    }

    public void moveHealthBarToFieldPosition()
    {
        RectTransform barRectTransform = gameObject.transform.GetComponent<RectTransform>();
        barRectTransform.anchoredPosition = new Vector2(barRectTransform.anchoredPosition.x, -160f);
    }

    private void decreaseHealthText(int damage)
    {
        if (damage < 0) { damage = 0; }
        int newHealth = Mathf.Max(currHealth - damage, 0);
        healthText.text = newHealth.ToString() + "/" + card.maxHealth;
    }

    private void adjustFillBars(int damage)
    {
        float proportionDamage = (float)damage / (float)card.maxHealth;
        float newGreenFill = currHealthProportion - proportionDamage;
        if (newGreenFill < 0) { newGreenFill = 0; }
        healthFillGreen.fillAmount = newGreenFill;
    }
}
