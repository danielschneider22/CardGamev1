using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NonCreatureCard")]
public class NonCreatureCard : Card
{
    public enum BorderColorType
    {
        attack = 0,
        defend = 1,
        other = 2,
    };
    public BorderColorType borderColorType;
    public enum target
    {
        playerCreature = 0,
        enemyCreature = 1,
        player = 2,
        enemy = 3
    }
    public delegate bool CanPlay(GameObject targetGameObject, Card card, PlayerController playerController);
    public CanPlay canPlay;
    public delegate void EnactEffect(GameObject targetGameObject);
    public EnactEffect enactEffect;
    public string text;
    public string frameText;
}
