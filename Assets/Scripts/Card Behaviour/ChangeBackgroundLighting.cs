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

    public void blackBacklighting()
    {
        backlightingImage.color = Color.black;
    }
    public void blueBacklighting()
    {
        backlightingImage.color = Color.blue;
    }
    public void whiteBacklighting()
    {
        backlightingImage.color = Color.white;
    }
}
