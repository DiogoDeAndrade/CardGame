using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    List<Card>  cards;

    public int GetCardCount()
    {
        if (cards == null) return 0;

        return cards.Count;
    }

    public void Add(Card card)
    {
        if (cards == null) cards = new List<Card>();

        card.transform.SetParent(transform);
        cards.Add(card);
    }
}
