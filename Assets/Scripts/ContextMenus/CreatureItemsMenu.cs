using System.Collections.Generic;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class CreatureItemsMenu : IContextMenu
    {
        private Dictionary<Consumable, IContextMenuItem> Actions { get; } = new();
        private Character Creature { get; }
        public ListInventory<Consumable> Pouch { get; }

        public string Title => "Items";
        public Color Color => ContextMenuAssets.Instance.ItemColor;
        public Sprite Icon => ContextMenuAssets.Instance.ItemIcon;
        public bool Enabled => !Creature.StatusEffectList.Contain<StunStatus>();

        public CreatureItemsMenu(Character creature, ListInventory<Consumable> pouch)
        {
            Creature = creature;
            Pouch = pouch;
        }

        public void Execute() { }

        public IEnumerable<IContextMenuItem> GetItems()
        {
            foreach (var slot in Pouch.slots)
            {
                if(!slot.item) continue;
                if (!Actions.TryGetValue(slot.item, out var action))
                    Actions[slot.item] = action = new UseItemMenuItem(Creature, Pouch, slot);
                yield return action;
            }

            yield return BackMenuItem.Item;
        }
    }
}