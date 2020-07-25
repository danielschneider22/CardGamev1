using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;

    public bool isDestroyed;

    public Sprite artwork;

    public int cardCost;
    [System.Serializable]
    public class CanPlayRequirement
    {
        public CanPlayRequirementName requirementName;
        public int requirementNumber;
    }
    public List<CanPlayRequirement> canPlayRequirements;

    public delegate bool CanPlay(ref bool canPlayTracker, GameObject targetGameObject, Card cardBeingPlayed, PlayerController playerController);
    public CanPlay canPlay;
}
