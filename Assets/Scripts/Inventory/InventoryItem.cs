using TricksAndTreatsOrThreats;
using UnityEngine;

public abstract class InventoryItem : DatabaseItem, IItem {
    [field: SerializeField] public string displayName { get; private set; }
    [field: SerializeField] public string description { get; private set; }

    public virtual int MaxStack => 99;
    public override string DisplayName => displayName;
    public override string Description => description;
    
    Sprite IItem.sprite => Icon;
    int IItem.maxStack => MaxStack;
}