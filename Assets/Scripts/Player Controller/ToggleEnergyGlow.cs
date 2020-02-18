using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEnergyGlow : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    public void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void stopGlow()
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
