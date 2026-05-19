using System.Collections.Generic;
using UnityEngine;

namespace TTT.ContextMenus {
    public class CharacterCombatMenu : IContextMenu {
        public string Title { get; }
        public Sprite Icon => null;
        public Color Color => Color.white;
        public bool Enabled => true;

        private List<IContextMenuItem> Actions { get; } = new ();
        public IEnumerable<IContextMenuItem> GetItems() => Actions;

        public CharacterCombatMenu(Character creature, ListInventory<Consumable> pouch) {
            Title = creature.Creature.DisplayName;
            Actions.Add(new AttackMenuItem(creature));
            Actions.Add(new DefendMenuItem(creature));
            Actions.Add(new CreatureSkillsMenu(creature));
            if (pouch != null) Actions.Add(new CreatureItemsMenu(creature, pouch));
        }
    }
}