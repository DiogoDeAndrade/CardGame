using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    List<Card>  cards;
    Player      player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        
    }

    public void Add(Card card)
    {
        if (cards == null) cards = new List<Card>();

        cards.Add(card);
        card.transform.SetParent(transform);
        card.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);

        card.desc.OnPlay(player);
        card.SetEventHandler(OnTap);
    }

    private void OnTap(Card card)
    {
        if (card.tapped) return;

        // Only creatures can be tapped
        if (card.desc.type != CardDesc.Type.Creature) return;

        card.Tap();

        if (GameMng.GetRules().attackOnTap)
        {
            player.RunAttack(card);
        }
    }

    public void RunUpkeep()
    {
        if (cards == null) return;

        foreach (var card in cards)
        {
            // Run per turn effect
            card.desc.OnUpkeep(player);

            // Untap card
            card.Untap();
        }
    }

    public List<Card> GetCards()
    {
        return cards;
    }

    public void ClearCreatures()
    {
        foreach (var card in cards)
        {
            if ((card.desc.type == CardDesc.Type.Creature) &&
                (card.defense <= 0))
            {
                // Creature is dead, delete it
                player.SendToGraveyard(card.desc);
                Destroy(card.gameObject);
            }
        }

        // Remove dead cards
        cards.RemoveAll((c) => (c == null) || (c.defense <= 0));
    }
}
