using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Card Game/Card")]
public class CardDesc : ScriptableObject
{
    public enum Type { Energy, Creature };

    public Type     type;
    public string   cardName = "No name";
    public int      cost = 0;

    [ShowIf("IsEnergy")]
    public int      energyAmmount = 0;

    [ShowIf("IsCreature")]
    public int      attackPower = 0;
    [ShowIf("IsCreature")]
    public int      defensePower = 0;

    [TextArea]
    public string   flavourText;
    [ShowAssetPreview(128, 128)]
    public Sprite   image;

    bool IsEnergy()
    {
        return type == Type.Energy;
    }

    bool IsCreature()
    {
        return type == Type.Creature;
    }

    public void OnPlay(Player player)
    {
        if ((type == Type.Energy) && (GameMng.GetRules().energyOnDrop))
        {
            player.ChangeEnergy(energyAmmount);
        }

        // Pay the cost
        player.ChangeEnergy(-cost);
    }

    public void OnUpkeep(Player player)
    {
        if (type == Type.Energy)
        {
            player.ChangeEnergy(energyAmmount);
        }
    }

    public bool CanPlay(Player player)
    {
        if (player.energy >= cost)
        {
            return true;
        }

        return false;
    }
}
