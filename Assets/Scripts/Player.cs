using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public CardDeck playerDeck;
    public string   playerName = "Player 1";
    public int      hp = 20;
    public int      energy = 0;

    public Color    activeColor;
    public Color    waitColor;

    [Header("References")]
    public Hand             hand;
    public PlayArea         playArea;
    public CardPile         deck;
    public CardPile         graveyard;
    public Transform        overlayContainer;
    public TextMeshProUGUI  playerNameRef;
    public TextMeshProUGUI  playerHPRef;
    public TextMeshProUGUI  playerEnergyRef;
    public Button           nextTurnButton;

    GraphicRaycaster graphicRaycaster;
    CanvasScaler     canvasScaler;
    int              drawCount;
    bool             activePlayer = false;

    Card                currentCard;
    CardDesc.TargetType targetType;

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
                DropCardBackToHand();
            }
        }
    }

    public void Setup()
    {
        // Shuffle deck
        List<CardDesc> shuffledDeck = new List<CardDesc>(playerDeck.cards);

        ShuffleDeck(shuffledDeck);

        deck.Add(shuffledDeck);

        hp = 20;
        energy = 0;

        UpdateStats();

        // Set me as active player, so I can draw cards
        activePlayer = true;

        for (int i = 0; i < GameMng.GetRules().startCardsOnHand; i++)
        {
            DrawCard(deck);
            // So we don't exceed the draw limit
            drawCount = 0;
        }

        activePlayer = false;
    }

    public void StartTurn()
    {
        if (activePlayer) return;

        activePlayer = true;

        //graphicRaycaster.enabled = true;
        drawCount = 0;

        playerNameRef.color = playerHPRef.color = playerEnergyRef.color = activeColor;

        nextTurnButton.interactable = true;

        // Upkeep - Get all cards in play and run them
        playArea.RunUpkeep();
    }

    public void FinishTurn()
    {
        if (!GameMng.GetRules().attackOnTap)
        {
            var cards = playArea.GetCards();

            foreach (var card in cards)
            {
                if ((card.desc.type == CardDesc.Type.Creature) && (card.tapped))
                {
                    RunAttack(card);
                }
            }
        }

        activePlayer = false;

        //graphicRaycaster.enabled = false;

        playerNameRef.color = playerHPRef.color = playerEnergyRef.color = waitColor;

        nextTurnButton.interactable = false;
    }

    public void DrawCard(CardPile pile)
    {
        // Can't draw cards if we're not the active player
        if (!activePlayer) return;

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

        // If this card is targeted spell, we need to enable "casting" on a target
        targetType = CardDesc.TargetType.None;
        if (card.desc.NeedsTarget())
        {
            targetType = card.desc.targetType;
        }
    }

    public void DropCardBackToHand()
    {
        // Can't reset the card to this hand if we're not the active player
        if (!activePlayer) return;

        if (currentCard == null) return;

        hand.Add(currentCard);
        currentCard = null;
    }

    public void DropCardInPlayArea()
    {
        // Can't drop a card in the other player's area
        if (!activePlayer) return;

        if (currentCard == null) return;

        // Check if we can drop this card (if we can pay the cost or other weirder criteria)
        if (currentCard.desc.CanPlay(this))
        {
            playArea.Add(currentCard);
            hand.Remove(currentCard);
            currentCard = null;
        }
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

    void UpdateStats()
    {
        playerNameRef.text = playerName;
        playerHPRef.text = "HP: " + hp + "/20";
        playerEnergyRef.text = "Energy: " + energy + "/20";
    }

    public void ChangeEnergy(int inEnergy)
    {
        energy = Math.Min(energy + inEnergy, 20);
        UpdateStats();
    }

    public void NextTurn()
    {
        GameMng.NextTurn();
    }

    public void RunAttack(Card card)
    {
        int     attackPower = card.GetAttackPower();
        Player  otherPlayer = GameMng.GetOtherPlayer(this);

        otherPlayer.DealDamage(this, attackPower);
    }

    public void DealDamage(Player player, int totalDamage)
    {
        int damage = totalDamage;

        var inPlay = playArea.GetCards();

        foreach (var card in inPlay)
        {
            if ((card.desc.type == CardDesc.Type.Creature) &&
                (!card.tapped))
            {
                // This creature can defend (damage can overflow)
                damage = card.DealDamage(damage);
                if (damage <= 0) break;
            }
        }

        playArea.ClearCreatures();

        if (damage > 0)
        {
            // Deal damage to the player itself
            hp -= damage;
            if (hp < 0)
            {
                // Player dead!
                Debug.Log(playerName + " is dead!");
            }
            UpdateStats();
        }
    }

    public void SendToGraveyard(CardDesc card)
    {
        graveyard.Add(card);
    }

    public bool IsPlayerActive()
    {
        return activePlayer;
    }

    public bool IsTargeting()
    {
        return targetType != CardDesc.TargetType.None;
    }

    public void RunAction(Card card)
    {
        if (currentCard == null) return;

        if (currentCard.desc.IsValidTarget(this, card))
        {
            // This is a valid target
            currentCard.desc.CastSpell(this, currentCard, card);

            // Destroy this card
            hand.Remove(currentCard);
            graveyard.Add(currentCard.desc);
            Destroy(currentCard.gameObject);

            // Clear the playfield
            playArea.ClearCreatures();
            GameMng.GetOtherPlayer(this).playArea.ClearCreatures();

            // Disable target mode
            targetType = CardDesc.TargetType.None;
        }
    }
}
