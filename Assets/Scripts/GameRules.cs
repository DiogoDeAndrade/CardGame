using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Card Game/Game Rules")]
public class GameRules : ScriptableObject
{
    public bool infiniteDeck = true;
    [ShowIf("infiniteDeck")]
    public bool reshuffleInfinite = true;
    public int  startCardsOnHand = 4;
    public int  maxCardsInHand = 6;
    public int  drawPerTurn = 1;
    public bool energyOnDrop = false;
    public bool attackOnTap = false;
}
