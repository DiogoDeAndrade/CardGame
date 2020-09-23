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
    }

    public void RunUpkeep()
    {
        if (cards == null) return;

        foreach (var card in cards)
        {
            card.desc.OnUpkeep(player);
        }
    }
}
