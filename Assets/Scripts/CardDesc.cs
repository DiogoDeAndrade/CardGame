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
    [TextArea]
    public string   flavourText;
    [ShowAssetPreview(128, 128)]
    public Sprite   image;
}
