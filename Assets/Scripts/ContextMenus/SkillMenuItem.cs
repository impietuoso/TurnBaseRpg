using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

namespace TTT.ContextMenus {
    public class SkillMenuItem : IContextMenuItem {
        public SkillMenuItem(Character user, Skill skill) {
            User = user;
            Skill = skill;
            PickerArgs = new (Validate, Enqueue)
            {
                User = user,
                Icon = Skill.icon,
                ArrowColor = Color.green,
            };
        }

        public Character User { get; }
        public Skill Skill { get; }
        public TargetPickerArgs PickerArgs { get; }
        public virtual string Title => Skill.skillName;
        public virtual Sprite Icon => Skill.icon;
        public virtual Color Color => ContextMenuAssets.Instance.SkillColor;
        public bool Enabled => User.derivedStats.mana.currentValue >= Skill.cost;
        
        public void Enqueue(ITarget tgt) => User.NextAction = new (Skill, User, tgt);
        public bool Validate(ITarget tgt) => Skill.animation.ValidateTarget(User, tgt);
        public void Execute() {
            if (Skill.animation.NeedTarget)
                TargetPicker.Instance.ShowArrow(PickerArgs);
            else User.NextAction = new (Skill, User, User);
        }
    }
}