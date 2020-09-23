﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CardDeck playerDeck;

    [Header("References")]
    public Hand     hand;
    public CardPile deck;
    public CardPile graveyard;

    GraphicRaycaster graphicRaycaster;
    int              drawCount;

    // Start is called before the first frame update
    void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        // Shuffle deck
        List<CardDesc> shuffledDeck = new List<CardDesc>(playerDeck.cards);

        ShuffleDeck(shuffledDeck);

        deck.Add(shuffledDeck);
    }

    public void StartTurn()
    {
        graphicRaycaster.enabled = true;
        drawCount = 0;
    }

    public void FinishTurn()
    {
        graphicRaycaster.enabled = false;
    }

    public void DrawCard(CardPile pile)
    {
        // Check if pile has cards
        if (pile.GetCardCount() <= 0)
        {
            // Check if we can dump the graveyard back in the deck
            if (GameMng.GetRules().infiniteDeck)
            {
                var cards = graveyard.GetCards();
                graveyard.Clear();

                if (GameMng.GetRules().reshuffleInfinite)
                {
                    ShuffleDeck(cards);
                }

                pile.Add(cards);

                if (pile.GetCardCount() <= 0) return;
            }
            else
            {
                return;
            }
        }

        // Check if we can draw cards (cards in hand is less than maximum allowed)
        if (hand.GetCardCount() >= GameMng.GetRules().maxCardsInHand) return;

        // Check if we've already drawn enough cards this turn
        if (drawCount >= GameMng.GetRules().drawPerTurn) return;

        drawCount++;

        // Get the first card
        CardDesc card = pile.GetFirstCard();

        // Create the card itself and pop it on the hand
        var cardObject = GameMng.CreateCard(card);

        hand.Add(cardObject);
    }

    void ShuffleDeck(List<CardDesc> cards)
    {
        for (int i = 0; i < 1000; i++)
        {
            int i1 = Random.Range(0, cards.Count);
            int i2 = Random.Range(0, cards.Count);
            var c = cards[i1];
            cards[i1] = cards[i2];
            cards[i2] = c;
        }
    }
}
