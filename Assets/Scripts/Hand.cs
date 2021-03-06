﻿using System;
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
        card.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);

        // Check if we're just returning the card to the hand, or if it's a new card
        if (!cards.Exists((c) => card == c))
        {
            cards.Add(card);
        }

        card.SetEventHandler(GrabCard);
    }

    public void Remove(Card card)
    {
        if (cards == null) return;

        cards.Remove(card);
    }

    private void GrabCard(Card card)
    {
        var player = GetComponentInParent<Player>();
        if (player.IsPlayerActive())
        {
            // Remove grab click handle
            card.SetEventHandler(null);

            // Pass the responsibility of the card to the player itself
            player.SetCard(card);
        }
    }
}
