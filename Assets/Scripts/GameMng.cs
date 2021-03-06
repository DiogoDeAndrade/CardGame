﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public List<Player>         players;
    public GameRules            rules;
    public Card                 emptyCardPrefab;

    int playerIndex = 0;

    static GameMng instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (var player in players)
        {
            player.Setup();
        }

        playerIndex = 0;
        StartTurn(playerIndex);
    }

    void Update()
    {
        
    }

    void StartTurn(int index)
    {
        foreach (var player in players)
        {
            player.FinishTurn();
        }

        players[index].StartTurn();
    }

    void _NextTurn()
    {
        playerIndex = (playerIndex + 1) % players.Count;

        StartTurn(playerIndex);
    }

    Player _GetOtherPlayer(Player player)
    {
        foreach (var p in players)
        {
            if (p != player) return p;
        }

        return null;
    }

    Player _GetActivePlayer()
    {
        foreach (var p in players)
        {
            if (p.IsPlayerActive()) return p;
        }

        return null;
    }

    static public GameRules GetRules()
    {
        return instance.rules;
    }

    static public Card CreateCard(CardDesc card)
    {
        Card cardObject = Instantiate(instance.emptyCardPrefab);

        cardObject.hidden = false;
        cardObject.name = "Card[" + card.name + "]";
        cardObject.Set(card);

        return cardObject;
    }

    static public void NextTurn()
    {
        instance._NextTurn();
    }

    static public Player GetOtherPlayer(Player player)
    {
        return instance._GetOtherPlayer(player);
    }

    static public Player GetActivePlayer()
    {
        return instance._GetActivePlayer();
    }
}
