using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPile : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI countText;
    public Image           cardDisplay;

    List<CardDesc>  cards;

    void Start()
    {
        UpdateCount();
    }

    void Update()
    {
        
    }

    public void Add(List<CardDesc> cardsToAdd)
    {
        if (cards == null) cards = new List<CardDesc>();

        cards.AddRange(cardsToAdd);
        UpdateCount();
    }

    public void Add(CardDesc cardToAdd)
    {
        if (cards == null) cards = new List<CardDesc>();

        cards.Add(cardToAdd);
        UpdateCount();
    }

    public void Clear()
    {
        cards = new List<CardDesc>();
        UpdateCount();
    }

    void UpdateCount()
    {
        int count = GetCardCount();

        countText.text = "Cards: " + count;
        cardDisplay.gameObject.SetActive(count > 0);
    }

    public int GetCardCount()
    {
        if (cards == null) return 0;

        return cards.Count;
    }

    public CardDesc GetFirstCard()
    {
        var card = cards[0];

        cards.RemoveAt(0);
        UpdateCount();

        return card;
    }

    public List<CardDesc> GetCards()
    {
        return cards;
    }
}
