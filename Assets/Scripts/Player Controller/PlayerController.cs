using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI energyText;
    public int currEnergy;
    public int maxEnergy;
    public int currHealth;
    public int maxHealth;
    public ToggleEnergyGlow toggleEnergyGlow;
    public FieldManager fieldManager;
    public Image playerShield;
    public Sprite shieldSprite;
    public Sprite brokenShieldSprite;
    public enum DefendingStatus { defended, notDefended, shieldsBroken }
    public DefendingStatus defendingStatus;
    public bool isEnemyController;

    public void Awake()
    {
        energyText.text = currEnergy.ToString() + "/" + maxEnergy.ToString();
        defendingStatus = DefendingStatus.notDefended;
    }

    public void Update()
    {
        bool playerDefended = isDefended();
        if (playerDefended && (defendingStatus == DefendingStatus.notDefended || defendingStatus == DefendingStatus.shieldsBroken))
        {
            playerShield.enabled = true;
            playerShield.sprite = shieldSprite;
            defendingStatus = DefendingStatus.defended;
        }
        else if (!playerDefended && defendingStatus == DefendingStatus.defended)
        {
            playerShield.enabled = true;
            playerShield.sprite = brokenShieldSprite;
            defendingStatus = DefendingStatus.shieldsBroken;
        }
        else if (defendingStatus == DefendingStatus.notDefended && !playerDefended && playerShield.enabled == true)
        {
            playerShield.enabled = false;
        }
    }

    public void decreaseCurrEnergy(int energyUsed)
    {
        currEnergy = currEnergy - energyUsed;
        energyText.text = currEnergy.ToString() + "/" + maxEnergy.ToString();
        if(currEnergy == 0)
        {
            toggleEnergyGlow.stopGlow();
        }
    }

    public void increaseCurrEnergy(int energyAdded)
    {
        if(currEnergy == 0 && energyAdded > 0)
        {
            toggleEnergyGlow.startGlow();
        }
        if (currEnergy + 1 <= maxEnergy)
        {
            currEnergy = currEnergy + 1;
        } else
        {
            currEnergy = maxEnergy;
        }
        energyText.text = currEnergy.ToString() + "/" + maxEnergy.ToString();
    }

    public void resetEnergy()
    {
        if (currEnergy == 0)
        {
            toggleEnergyGlow.startGlow();
        }
        currEnergy = maxEnergy;
        energyText.text = currEnergy.ToString() + "/" + maxEnergy.ToString();
    }
    public bool isDefended()
    {

        return fieldManager.fieldHasDefendingCard();
    }
}
