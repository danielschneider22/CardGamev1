using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentionManager : MonoBehaviour
{
    public List<GameObject> cards;
    public List<EnemyAction> enemyActions;
    public GameObject creatureCardTemplate;
    public GridLayoutGroup enemyField;
    public PlayerController enemyController;
    public DraggableArrow draggableArrow;
    public GameObject enemyFieldBackground;
    public Camera cam;

    private float enemyActionTimer;
    private void Awake()
    {
        enemyActionTimer = 0f;
        enemyActions = new List<EnemyAction>{ };
    }

    public void addCard(GameObject card)
    {
        cards.Add(card);
    }

    public void Update()
    {
        if(enemyActions.Count > 0 && enemyActionTimer <= 0f)
        {
            EnemyAction enemyAction = enemyActions[0];
            if(!enemyAction.showingHover && !enemyAction.showingArrow)
            {
                showHover(enemyAction);
            }
            // playing creature
            else if (enemyAction.actionType == EnemyActionType.playCreature && enemyAction.showingHover && !enemyAction.showingArrow)
            {
                drawIntentArrow(enemyAction, cam.WorldToScreenPoint(enemyField.transform.position));
                enemyFieldBackground.SetActive(true);
            } else if (enemyAction.actionType == EnemyActionType.playCreature && enemyAction.showingArrow)
            {
                placeCardInEnemyField(enemyAction.card);
                removeAction(enemyAction);
                enemyFieldBackground.SetActive(false);
            }
            // playing attack or defend card
            else if ((enemyAction.actionType == EnemyActionType.playAttack || enemyAction.actionType == EnemyActionType.playDefend) && enemyAction.showingHover && !enemyAction.showingArrow)
            {
                drawIntentArrow(enemyAction, cam.WorldToScreenPoint(new Vector3(enemyAction.cardTarget.transform.position.x, enemyAction.card.transform.position.y, enemyAction.card.transform.position.z)));
                enemyAction.cardTarget.GetComponent<ChangeBackgroundLighting>().greenBacklighting();
            }
            else if ((enemyAction.actionType == EnemyActionType.playAttack || enemyAction.actionType == EnemyActionType.playDefend) && enemyAction.showingArrow)
            {
                Card enemyCard = enemyAction.card.GetComponent<CardDisplay>().card;
                if(enemyAction.actionType == EnemyActionType.playAttack)
                {
                    enemyAction.cardTarget.GetComponent<MakeCardAttack>().activateAttack((AttackCard)enemyCard);
                } else
                {
                    enemyAction.cardTarget.GetComponent<MakeCardDefend>().activateDefend((DefendCard)enemyCard);
                }
                enemyAction.cardTarget.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();

                enemyController.decreaseCurrEnergy(enemyCard.cardCost);
                removeAction(enemyAction);
                Destroy(enemyAction.card);
            }
        } else if (enemyActions.Count> 0)
        {
            enemyActionTimer -= Time.deltaTime;
        }
    }
    private void drawIntentArrow(EnemyAction enemyAction, Vector3 endPoint)
    {
        draggableArrow.startPos = cam.WorldToScreenPoint(new Vector3(enemyAction.card.transform.position.x, enemyAction.card.transform.position.y + .3f, enemyAction.card.transform.position.z));
        draggableArrow.staticEndPos = endPoint;
        draggableArrow.drawStaticArrow = true;
        enemyActionTimer = 2f;
        enemyActions[0].showingArrow = true;
    }

    private void showHover(EnemyAction enemyAction)
    {
        enemyAction.card.GetComponent<ChangeBackgroundLighting>().whiteBacklighting();
        enemyActionTimer = 1f;
        enemyActions[0].showingHover = true;
    }

    private void removeAction(EnemyAction enemyAction)
    {
        enemyActions.Remove(enemyAction);
        draggableArrow.drawStaticArrow = false;
        draggableArrow.staticEndPos = null;
        draggableArrow.clearArrow();
        enemyActionTimer = 1f;
    }

    public void enactEnemyIntent()
    {
        enemyActions = getEnemyActions();
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
        enemyController.decreaseCurrEnergy(newChild.GetComponent<CardDisplay>().card.cardCost);
        foreach(EnemyAction enemyAction in enemyActions)
        {
            if(enemyAction.card == cardObj)
            {
                enemyAction.card = newChild;
            } else if(enemyAction.cardTarget == cardObj)
            {
                enemyAction.cardTarget = newChild;
            }
        }
        Destroy(cardObj);
    }

    private List<EnemyAction> getEnemyActions()
    {
        EnemyAction playCreature = new EnemyAction(cards[0], null, EnemyActionType.playCreature);
        EnemyAction playCreature2 = new EnemyAction(cards[1], null, EnemyActionType.playCreature);
        EnemyAction activateAttack = new EnemyAction(cards[2], cards[0], EnemyActionType.playAttack);


        List<EnemyAction> actions = new List<EnemyAction> { playCreature, playCreature2, activateAttack };

        return (actions);
    }
}
