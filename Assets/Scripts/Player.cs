using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CardDeck playerDeck;

    [Header("References")]
    public Hand     hand;
    public CardPile deck;
    public CardPile graveyard;

    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        canvasGroup.interactable = true;
    }

    public void FinishTurn()
    {
        canvasGroup.interactable = false;
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

        CardDesc card = pile.GetFirstCard();
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
