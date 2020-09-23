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
}
