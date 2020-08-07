
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CardDisplay;
using static NonCreatureCard;

public class EnemyIntentionManager : MonoBehaviour
{
    public List<GameObject> cards;
    public List<EnemyAction> enemyActions;
    public GameObject creatureCardTemplate;
    public FieldManager enemyFieldManager;
    public GameObject fieldPosition;
    public PlayerController enemyController;
    public DraggableArrow draggableArrow;
    public GameObject enemyFieldBackground;
    public FieldManager playerFieldManager;
    public PlayerController playerController;
    public Camera cam;
    public TurnManager turnManager;
    public AttackedPlayerManager attackedPlayerManager;
    private bool enemyTurnOver;
    private CanPlayManager canPlayManager;

    private float enemyActionTimer;
    private void Awake()
    {
        enemyActionTimer = 0f;
        enemyActions = new List<EnemyAction>{ };
        enemyTurnOver = true;
        canPlayManager = new CanPlayManager();
    }

    public void addCard(GameObject card)
    {
        cards.Add(card);
    }

    public void Update()
    {
        if(enemyActionTimer <= 0f)
        {
            if(enemyActions.Count > 0)
            {
                EnemyAction enemyAction = enemyActions[0];
                if (!enemyAction.showingHover && !enemyAction.showingArrow)
                {
                    showHover(enemyAction);
                }
                // playing creature
                else if (enemyAction.actionType == EnemyActionType.playCreature && enemyAction.showingHover && !enemyAction.showingArrow)
                {
                    drawIntentArrow(enemyAction, cam.WorldToScreenPoint(fieldPosition.transform.position));
                    enemyFieldBackground.SetActive(true);
                }
                else if (enemyAction.actionType == EnemyActionType.playCreature && enemyAction.showingArrow)
                {
                    placeCardInEnemyField(enemyAction.card);
                    removeAction(enemyAction);
                    enemyFieldBackground.SetActive(false);
                }
                // playing attack or defend card
                else if (enemyAction.actionType == EnemyActionType.playNonCreatureCard && enemyAction.showingHover && !enemyAction.showingArrow)
                {
                    drawIntentArrow(enemyAction, cam.WorldToScreenPoint(new Vector3(enemyAction.cardTarget.transform.position.x, enemyAction.card.transform.position.y + .3f, enemyAction.card.transform.position.z)));
                    enemyAction.cardTarget.GetComponent<ChangeBackgroundLighting>().greenBacklighting();
                }
                else if (enemyAction.actionType == EnemyActionType.playNonCreatureCard && enemyAction.showingArrow)
                {
                    NonCreatureCard enemyCard = (NonCreatureCard)enemyAction.card.GetComponent<CardDisplay>().card;
                    NonCreatureEffectsManager.enactNonCreatureEffect(enemyCard.effects, enemyAction.cardTarget, enemyController);
                    enemyAction.cardTarget.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();

                    removeAction(enemyAction);
                    Destroy(enemyAction.card);
                }
                // attacking a creature to another creature
                else if ((enemyAction.actionType == EnemyActionType.attackCreature) && enemyAction.showingHover && !enemyAction.showingArrow)
                {
                    drawIntentArrow(enemyAction, cam.WorldToScreenPoint(new Vector3(enemyAction.cardTarget.transform.position.x, enemyAction.card.transform.position.y, enemyAction.card.transform.position.z)));
                    enemyAction.cardTarget.GetComponent<ChangeBackgroundLighting>().greenBacklighting();
                    CreatureCard enemyCard = (CreatureCard)enemyAction.cardTarget.GetComponent<CardDisplay>().card;
                    enemyAction.cardTarget.GetComponent<AttackedManager>().tempReduceHealth(enemyCard);
                    enemyAction.cardTarget.GetComponent<AttackDefenseManager>().decreaseDefense(enemyCard.currAttack);
                }
                else if ((enemyAction.actionType == EnemyActionType.attackCreature) && enemyAction.showingArrow)
                {
                    CreatureCard enemyCard = (CreatureCard)enemyAction.card.GetComponent<CardDisplay>().card;
                    enemyAction.cardTarget.GetComponent<AttackedManager>().attackingCardObj = enemyAction.card;
                    enemyAction.cardTarget.GetComponent<AttackedManager>().applyTempAttack(enemyCard);

                    AttackDefenseManager atckDefManager = enemyAction.card.GetComponent<AttackDefenseManager>();
                    atckDefManager.cantAttack();

                    enemyAction.cardTarget.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();
                    enemyAction.card.GetComponent<ChangeBackgroundLighting>().nonselectableBacklighting();

                    enemyController.decreaseCurrEnergy(enemyCard.cardCost);
                    removeAction(enemyAction);
                }
                // attacking a creature to a player
                else if ((enemyAction.actionType == EnemyActionType.attackPlayer) && enemyAction.showingHover && !enemyAction.showingArrow)
                {
                    drawIntentArrow(enemyAction, attackedPlayerManager.gameObject.transform.position);
                    attackedPlayerManager.backgroundLighting.greenBacklighting();
                    CreatureCard enemyCard = (CreatureCard)enemyAction.card.GetComponent<CardDisplay>().card;
                    attackedPlayerManager.tempReduceHealth(enemyCard);
                }
                else if ((enemyAction.actionType == EnemyActionType.attackPlayer) && enemyAction.showingArrow)
                {
                    CreatureCard enemyCard = (CreatureCard)enemyAction.card.GetComponent<CardDisplay>().card;
                    attackedPlayerManager.attackingCardObj = enemyAction.card;
                    attackedPlayerManager.applyTempAttack(enemyCard);

                    AttackDefenseManager atckDefManager = enemyAction.card.GetComponent<AttackDefenseManager>();
                    atckDefManager.cantAttack();

                    attackedPlayerManager.backgroundLighting.transparentBacklighting();

                    enemyController.decreaseCurrEnergy(enemyCard.cardCost);
                    removeAction(enemyAction);
                }
                if (enemyActions.Count == 0)
                {
                    enemyActionTimer = 2f;
                    
                }
            }
            else if (!enemyTurnOver)
            {
                foreach (GameObject cardObj in cards)
                {
                    Destroy(cardObj);
                }
                cards = new List<GameObject>();
                turnManager.startPlayerTurn();
                enemyTurnOver = true;
            }
        } else if (enemyActionTimer > 0f)
        {
            enemyActionTimer -= Time.deltaTime;
        }
    }
    private void drawIntentArrow(EnemyAction enemyAction, Vector3 endPoint)
    {
        draggableArrow.startPos = cam.WorldToScreenPoint(new Vector3(enemyAction.card.transform.position.x, enemyAction.card.transform.position.y + .3f, enemyAction.card.transform.position.z));
        draggableArrow.staticEndPos = endPoint;
        draggableArrow.drawStaticArrow = true;
        enemyActionTimer = .5f;
        enemyActions[0].showingArrow = true;
    }

    private void showHover(EnemyAction enemyAction)
    {
        enemyAction.card.GetComponent<ChangeBackgroundLighting>().whiteBacklighting();
        enemyActionTimer = .5f;
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
        enemyTurnOver = false;
    }
    private GameObject simulatePlaceInEnemyField(GameObject cardObj)
    {
        creatureCardTemplate.SetActive(false);
        GameObject newChild = Instantiate(creatureCardTemplate);
        newChild.GetComponent<CardDisplay>().card = Instantiate(cardObj.GetComponent<CardDisplay>().card);
        newChild.GetComponent<CardDisplay>().card.cardCost = 100;
        newChild.GetComponent<CardDisplay>().material = cardObj.GetComponent<CardDisplay>().material;
        newChild.GetComponent<CardDisplay>().location = Location.enemyField;
        newChild.SetActive(true);
        newChild.GetComponent<EnergizedManager>().energize();
        creatureCardTemplate.SetActive(true);
        return newChild;
    }

    private void placeCardInEnemyField(GameObject cardObj)
    {
        creatureCardTemplate.SetActive(false);
        GameObject newChild = Instantiate(creatureCardTemplate);
        newChild.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        newChild.GetComponent<CardDisplay>().card = cardObj.GetComponent<CardDisplay>().card;
        newChild.GetComponent<CardDisplay>().material = cardObj.GetComponent<CardDisplay>().material;
        newChild.GetComponent<CardDisplay>().location = Location.enemyField;
        newChild.GetComponent<ToggleVisibility>().makeVisible();
        newChild.GetComponent<CanvasGroup>().blocksRaycasts = true;
        newChild.transform.localScale = cardObj.transform.localScale;
        newChild.SetActive(true);
        newChild.GetComponent<EnergizedManager>().energize();
        newChild.transform.position = cardObj.transform.position;
        enemyFieldManager.addCardToField(newChild, false);

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
    private int getBestTargetNonCreatureHelpfulIdx(NonCreatureCard nonCreatureCard, List<GameObject> enemyFieldCards, PlayerController enemyController)
    {
        int idx = 0;
        foreach(GameObject cardObj in enemyFieldCards)
        {
            if(CanPlayManager.canPlay(cardObj, nonCreatureCard, enemyController))
            {
                return idx;
            }
            idx++;
        }
        return -1;
    }

    private List<EnemyAction> getActionsBasedOnDanielsDumbAI()
    {
        List<EnemyAction> enemyActions = new List<EnemyAction>();
        PlayerController tempEnemyController = Instantiate(enemyController);
        List<GameObject> tempFieldGameObjects = enemyFieldManager.getFieldGameObjects().ConvertAll(item => Instantiate(item));
        List<GameObject> actualEnemyFieldGameObjs = enemyFieldManager.getFieldGameObjects();

        // first play creatures
        foreach (GameObject cardObj in cards)
        {
            Card card = cardObj.GetComponent<CardDisplay>().card;
            if (card is CreatureCard && CanPlayManager.canPlay(null, card, tempEnemyController))
            {
                tempEnemyController.currEnergy = tempEnemyController.currEnergy - card.cardCost;
                enemyActions.Add(new EnemyAction(cardObj, null, EnemyActionType.playCreature));
                actualEnemyFieldGameObjs.Add(cardObj);
                GameObject copyCardObj = Instantiate(cardObj);
                copyCardObj = simulatePlaceInEnemyField(copyCardObj);
                tempFieldGameObjects.Add(Instantiate(copyCardObj));
            }
        }

        // then play non-creatures
        foreach (GameObject cardObj in cards)
        {
            Card card = cardObj.GetComponent<CardDisplay>().card;
            if (card is NonCreatureCard)
            {
                NonCreatureCard nonCreatureCard = (NonCreatureCard)card;
                if (nonCreatureCard.target == Target.controllerCreature)
                {
                    int bestCardToPlayIdx = getBestTargetNonCreatureHelpfulIdx(nonCreatureCard, tempFieldGameObjects, tempEnemyController);
                    if(bestCardToPlayIdx != -1)
                    {
                        enemyActions.Add(new EnemyAction(cardObj, actualEnemyFieldGameObjs[bestCardToPlayIdx], EnemyActionType.playNonCreatureCard));
                        tempEnemyController.currEnergy = tempEnemyController.currEnergy - card.cardCost;
                        NonCreatureEffectsManager.enactNonCreatureEffect(nonCreatureCard.effects, tempFieldGameObjects[bestCardToPlayIdx], enemyController);
                    }
                    
                }
            }
        }

        // choose which creatures to attack
        int idx = 0;
        foreach (GameObject cardObj in tempFieldGameObjects)
        {
            CreatureCard card = (CreatureCard)cardObj.GetComponent<CardDisplay>().card;
            GameObject bestCardObjToAttck = playerFieldManager.getBestDefendingCreatureToAttack();
            if (card.canAttack && bestCardObjToAttck != null)
            {
                enemyActions.Add(new EnemyAction(actualEnemyFieldGameObjs[idx], bestCardObjToAttck, EnemyActionType.attackCreature));
            }
            else if (card.canAttack && bestCardObjToAttck == null)
            {
                enemyActions.Add(new EnemyAction(actualEnemyFieldGameObjs[idx], null, EnemyActionType.attackPlayer));
            }
            idx++;
        }

        return enemyActions;
    }

    private List<EnemyAction> getEnemyActions()
    {
        List<EnemyAction> enemyActions = getActionsBasedOnDanielsDumbAI();

        return (enemyActions);
    }
}
