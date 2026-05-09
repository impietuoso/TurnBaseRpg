using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class UseSkillMenuItem : IContextMenuItem
    {
        public UseSkillMenuItem(Character user, Skill skill)
        {
            User = user;
            Skill = skill;
        }

        public Character User { get; }
        public Skill Skill { get; }
        public string Title => Skill.skillName;
        public Sprite Icon => Skill.icon;
        public Color Color => ContextMenuAssets.Instance.SkillColor;
        public bool Enabled => User.derivedStats.mana.currentValue >= Skill.cost;
        public void Execute() => TargetPicker.Instance.ShowArrow(User.transform, Color.grey);
    }
}