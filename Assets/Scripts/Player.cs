using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CardDeck playerDeck;

    [Header("References")]
    public Hand         hand;
    public CardPile     deck;
    public CardPile     graveyard;
    public Transform    overlayContainer;

    GraphicRaycaster graphicRaycaster;
    CanvasScaler     canvasScaler;
    int              drawCount;

    Card             currentCard;

    // Start is called before the first frame update
    void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCard)
        {
            // Move card with the cursor
            var mousePos = Input.mousePosition;

            // Normalize mouse position (Y coordinate is inverted from screen space to the current viewport setup I have)
            mousePos.x = (mousePos.x / Screen.width);
            mousePos.y = 1.0f - (mousePos.y / Screen.height);

            // Convert mouse position to canvas coordinates (Y coordinate grows up, so we have to negate it)
            mousePos.x = canvasScaler.referenceResolution.x * mousePos.x;
            mousePos.y = -canvasScaler.referenceResolution.y * mousePos.y;

            currentCard.GetComponent<RectTransform>().anchoredPosition = mousePos;

            // Right button clicked, return the current card to the hand
            if (Input.GetMouseButtonUp(1))
            {
                hand.Add(currentCard);
                currentCard = null;
            }
        }
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
        // Can't draw a card if we have one floating
        if (currentCard != null) return;

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

    public void SetCard(Card card)
    {
        // Check if we already have a card, if so, return this one to the hand
        if (currentCard)
        {
            hand.Add(currentCard);
        }

        // Change the parent, this is now a floating card
        card.transform.SetParent(overlayContainer);
        card.transform.localScale = new Vector3(2.5f, 2.5f, 2.0f);

        // Card movement
        currentCard = card;
    }

    private void DropCard(Card card)
    {
    }

    void ShuffleDeck(List<CardDesc> cards)
    {
        for (int i = 0; i < 1000; i++)
        {
            int i1 = UnityEngine.Random.Range(0, cards.Count);
            int i2 = UnityEngine.Random.Range(0, cards.Count);
            var c = cards[i1];
            cards[i1] = cards[i2];
            cards[i2] = c;
        }
    }
}
