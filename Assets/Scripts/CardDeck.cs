using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Game/Deck")]
public class CardDeck : ScriptableObject
{
    public List<CardDesc>   cards;
}
