using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleEnergyGlow : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    public Image energyImage;
    public Sprite greyEnergySprite;
    public Sprite energySprite;
    public void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void stopGlow()
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        energyImage.sprite = greyEnergySprite;
    }
    public void startGlow()
    {
        particleSystem.Play();
        energyImage.sprite = energySprite;
    }
}
