using System.Collections;
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

    private Card card;

    public void Awake()
    {
        card = cardDisplay.card;
    }

    private float currHealthProportion;
    private int currHealth;
    private int tempDamage;
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
        card.currHealth = card.currHealth - tempDamage;
        return card.currHealth <= 0;
    }

    private void decreaseHealthText(int damage)
    {
        int newHealth = currHealth - damage;
        if (damage < 0) { damage = 0; }
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
