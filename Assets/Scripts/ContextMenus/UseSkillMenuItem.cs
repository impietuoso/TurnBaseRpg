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
            PickerArgs = new(Validate, Cast)
            {
                User = user,
                Icon = Skill.icon,
                ArrowColor = Color.green,
            };
        }

        public Character User { get; }
        public Skill Skill { get; }
        public TargetPickerArgs PickerArgs { get; }
        public string Title => Skill.skillName;
        public Sprite Icon => Skill.icon;
        public Color Color => ContextMenuAssets.Instance.SkillColor;
        public bool Enabled => User.derivedStats.mana.currentValue >= Skill.cost;
        public void Execute() => TargetPicker.Instance.ShowArrow(PickerArgs);
        public void Cast(ITarget tgt) => User.NextAction= new(User,tgt, Skill);
        public bool Validate(ITarget tgt) => Skill.animation.ValidateTarget(User, tgt);
    }
}