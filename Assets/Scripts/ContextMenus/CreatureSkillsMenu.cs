using System.Collections.Generic;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class CreatureSkillsMenu : IContextMenu
    {
        private Dictionary<ScriptableObject, IContextMenuItem> Actions { get; } = new();
        private Character Creature { get; }
        public string Title => "Skill";
        public Sprite Icon => ContextMenuAssets.Instance.SkillIcon;
        public Color Color => ContextMenuAssets.Instance.SkillColor;
        public bool Enabled => !Creature.StatusEffectList.Contain<Silence>();
        public CreatureSkillsMenu(Character creature) => Creature = creature;

        public void Execute() { }

        public IEnumerable<IContextMenuItem> GetItems()
        {
            foreach (var skill in Creature.skills)
            {
                if (!Actions.TryGetValue(skill, out var action))
                    Actions[skill] = action = new UseSkillMenuItem(Creature, skill);
                yield return action;
            }

            yield return BackMenuItem.Item;
        }
    }
}