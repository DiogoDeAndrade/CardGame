using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CardDeck playerDeck;

    [Header("References")]
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
        for (int i = 0; i < 1000; i++)
        {
            int i1 = Random.Range(0, shuffledDeck.Count);
            int i2 = Random.Range(0, shuffledDeck.Count);
            var c = shuffledDeck[i1];
            shuffledDeck[i1] = shuffledDeck[i2];
            shuffledDeck[i2] = c;
        }
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
}
