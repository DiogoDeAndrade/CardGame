using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Card Game/Card")]
public class CardDesc : ScriptableObject
{
    public enum Type { Energy, Creature, TargetedSpell };
    public enum EffectType { Damage, Heal };
    public enum TargetType { None, EnemyCreature, EnemyCard, PlayerCreature, PlayerCard };

    public Type     type;
    public string   cardName = "No name";
    public int      cost = 0;

    [ShowIf("IsEnergy")]
    public int      energyAmmount = 0;

    [ShowIf("IsCreature")]
    public int      attackPower = 0;
    public int      defensePower = 1;

    [ShowIf("IsSpell")]
    public EffectType   effectType = EffectType.Damage;
    [ShowIf("NeedsTarget")]
    public TargetType   targetType = TargetType.None;
    [ShowIf("IsDamageSpell")]
    public int          damage;
    [ShowIf("IsHealingSpell")]
    public int          healAmmount;

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

    bool IsSpell()
    {
        return type == Type.TargetedSpell;
    }

    bool IsDamageSpell()
    {
        return IsSpell() && effectType == EffectType.Damage;
    }

    bool IsHealingSpell()
    {
        return IsSpell() && effectType == EffectType.Heal;
    }

    bool IsTargetedSpell()
    {
        return type == Type.TargetedSpell;
    }

    public bool NeedsTarget()
    {
        return type == Type.TargetedSpell;
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

    public bool IsValidTarget(Player player, Card card)
    {
        if (card.GetComponentInParent<Player>() != player)
        {
            // Enemy card
            if ((targetType == TargetType.EnemyCreature) && (card.desc.type == Type.Creature)) return true;
            if (targetType == TargetType.EnemyCard) return true;
        }
        else
        {
            // Friend card
            if ((targetType == TargetType.PlayerCreature) && (card.desc.type == Type.Creature)) return true;
            if (targetType == TargetType.PlayerCard) return true;
        }

        return false;
    }

    public void CastSpell(Player srcPlayer, Card srcCard, Card destCard)
    {
        switch (effectType)
        {
            case EffectType.Damage:
                destCard.DealDamage(damage);
                break;
            case EffectType.Heal:
                destCard.Heal(healAmmount);
                break;
            default:
                break;
        }
    }
}
