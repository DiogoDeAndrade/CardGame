using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Game/Card")]
public class CardDesc : ScriptableObject
{
    public enum Type { Energy, Creature };

    public Type     type;
    public string   cardName = "No name";
    [TextArea]
    public string   flavourText;
}
