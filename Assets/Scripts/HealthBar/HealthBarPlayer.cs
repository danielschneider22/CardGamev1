using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : MonoBehaviour
{
    public Image healthFillGreen;
    public Image healthFillRed;
    public TextMeshProUGUI healthText;

    public PlayerController playerController;

    private float currHealthProportion;
    private int currHealth;
    public int tempDamage;
    public void tempDecreaseHealth(int damage)
    {
        tempDamage = damage;
        currHealth = playerController.currHealth;
        currHealthProportion = healthFillGreen.fillAmount;

        decreaseHealthText(damage);
        adjustFillBars(damage);
    }

    public void restoreTempHealth()
    {
        healthFillGreen.fillAmount = currHealthProportion;
        healthText.text = currHealth.ToString() + "/" + playerController.maxHealth;
    }
    
    // returns if it should be destroyed
    public bool applyTempDecreaseHealth()
    {
        healthFillRed.fillAmount = 1 - healthFillGreen.fillAmount;
        playerController.currHealth = playerController.currHealth - tempDamage;
        return playerController.currHealth <= 0;
    }

    public void moveHealthBarToFieldPosition()
    {
        RectTransform barRectTransform = gameObject.transform.GetComponent<RectTransform>();
        barRectTransform.anchoredPosition = new Vector2(barRectTransform.anchoredPosition.x, -160f);
    }

    private void decreaseHealthText(int damage)
    {
        int newHealth = currHealth - damage;
        if (damage < 0) { damage = 0; }
        healthText.text = newHealth.ToString() + "/" + playerController.maxHealth;
    }

    private void adjustFillBars(int damage)
    {
        float proportionDamage = (float)damage / (float)playerController.maxHealth;
        float newGreenFill = currHealthProportion - proportionDamage;
        if (newGreenFill < 0) { newGreenFill = 0; }
        healthFillGreen.fillAmount = newGreenFill;
    }
}
