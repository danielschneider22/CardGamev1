using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI energyText;
    public int currEnergy;
    public int maxEnergy;
    public ToggleEnergyGlow toggleEnergyGlow;

    public void Awake()
    {
        energyText.text = currEnergy.ToString() + "/" + maxEnergy.ToString();
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
}
