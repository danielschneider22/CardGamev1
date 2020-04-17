using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentionManager : MonoBehaviour
{
    public List<GameObject> cards;
    public GameObject creatureCardTemplate;
    public GridLayoutGroup enemyField;

    public void addCard(GameObject card)
    {
        cards.Add(card);
    }

    public void enactEnemyIntent()
    {
        EnemyAction[] enemyActions = getEnemyActions();
        foreach(EnemyAction enemyAction in enemyActions)
        {
            if(enemyAction.actionType == EnemyActionType.playCreature)
            {
                placeCardInEnemyField(enemyAction.card);
            }
        }
    }

    private void placeCardInEnemyField(GameObject cardObj)
    {
        creatureCardTemplate.SetActive(false);
        GameObject newChild = Instantiate(creatureCardTemplate);
        newChild.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        newChild.GetComponent<CardDisplay>().card = cardObj.GetComponent<CardDisplay>().card;
        newChild.GetComponent<CardDisplay>().material = cardObj.GetComponent<CardDisplay>().material;
        newChild.GetComponent<CardDisplay>().location = "field";
        newChild.GetComponent<ToggleVisibility>().makeVisible();
        newChild.GetComponent<CanvasGroup>().blocksRaycasts = true;
        newChild.transform.localScale = new Vector3(1f, 1f, 1);
        newChild.SetActive(true);
        newChild.transform.SetParent(enemyField.transform, false);

        // float halfHeight = newChild.GetComponent<RectTransform>().rect.height / 2;
        // newChild.transform.position = playerFieldImage.transform.position;// Camera.main.ScreenToWorldPoint(cardObj.transform.position); // new Vector3(Input.mousePosition.x, Input.mousePosition.y - halfHeight, Input.mousePosition.z);
        // playerFieldManager.addCardToField(newChild);
        creatureCardTemplate.SetActive(true);
        Destroy(cardObj);
    }

    private EnemyAction[] getEnemyActions()
    {
        EnemyAction playCreature = new EnemyAction(cards[0], null, EnemyActionType.playCreature);

        EnemyAction[] actions = new EnemyAction[] { playCreature };

        return (actions);
    }
}
