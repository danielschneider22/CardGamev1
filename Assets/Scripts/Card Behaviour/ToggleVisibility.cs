using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{

    public GameObject drawnComponents;
    public GameObject textComponents;
    public void makeInvisible()
    {
        drawnComponents.SetActive(false);
        textComponents.SetActive(false);
    }

    public void makeVisible()
    {
        drawnComponents.SetActive(true);
        textComponents.SetActive(true);
    }
}
