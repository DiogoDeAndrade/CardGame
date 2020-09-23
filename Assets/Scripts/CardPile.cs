using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPile : MonoBehaviour
{
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

    void UpdateCount()
    {
        int count = 0;

        if (cards != null)
        {
            count = cards.Count;
        }

        countText.text = "Cards: " + count;
        cardDisplay.enabled = count > 0;
    }
}
