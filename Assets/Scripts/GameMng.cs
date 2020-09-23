using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public List<Player>         players;
    public GameRules            rules;

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

    static public GameRules GetRules()
    {
        return instance.rules;
    }
}
