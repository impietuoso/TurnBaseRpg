using System;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class UseItemMenuItem : IContextMenuItem
    {
        public UseItemMenuItem(Character user, Slot<Consumable> slot)
        {
            User = user;
            Slot = slot;
        }

        public Character User { get; }
        public Slot<Consumable> Slot { get; }
        public string Title => Slot.item.displayName + " x" + Slot.amount;
        public Sprite Icon => Slot.item.sprite;
        public Color Color => ContextMenuAssets.Instance.ItemColor;
        public bool Enabled => Slot.amount > 0;
        public void Execute() => throw new NotImplementedException();
    }
}