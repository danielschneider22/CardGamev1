using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackgroundLighting : MonoBehaviour
{
    public Image backlightingImage;

    public void greenBacklighting()
    {
        backlightingImage.color = Color.green;
    }

    public void nonselectableBacklighting()
    {
        backlightingImage.color = Color.black;
    }
    public void hoverBacklighting()
    {
        backlightingImage.color = new Color(0, .4827f, .9716f);
    }
    public void whiteBacklighting()
    {
        backlightingImage.color = Color.white;
    }
    public void redBacklighting()
    {
        backlightingImage.color = Color.red;
    }
    public void selectableBacklighting()
    {
        backlightingImage.color = new Color(0.6775098f, 0.9541942f, 0.9905f);
    }
}
