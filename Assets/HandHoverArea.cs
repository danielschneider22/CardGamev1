using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandHoverArea : MonoBehaviour, IPointerExitHandler
{
    private ChangeBackgroundLighting backgroundLighting;
    private RectTransform rectTransform;
    private Color initBacklightColor;
    private HandManager handManager;

    private void Awake()
    {
        // backgroundLighting = GetComponent<ChangeBackgroundLighting>();
        rectTransform = GetComponent<RectTransform>();
        handManager = GameObject.FindGameObjectWithTag("Hand Manager").GetComponent<HandManager>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        string parentObjName = gameObject.transform.parent.name;
        // backgroundLighting.backlightingImage.color = initBacklightColor;

        if (parentObjName == "TopOfHandArea")
        {
            handManager.resetHandPositions(500);
        }
    }
}
