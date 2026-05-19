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
                Icon = Skill.Icon,
                ArrowColor = Color.green,
            };
        }

        public Character User { get; }
        public Skill Skill { get; }
        private TargetPickerArgs PickerArgs { get; }
        public virtual string Title => Skill.DisplayName;
        public virtual Sprite Icon => Skill.Icon;
        public virtual Color Color => ContextMenuAssets.Instance.SkillColor;
        public bool Enabled => User.Mana.Current >= Skill.Cost;

        private void Enqueue(ITarget tgt) => User.NextAction = new (Skill, User, tgt);
        private bool Validate(ITarget tgt) => Skill.animation.ValidateTarget(User, tgt);
        public void Execute() {
            if (Skill.animation.NeedTarget)
                TargetPicker.Instance.ShowArrow(PickerArgs);
            else User.NextAction = new (Skill, User, User);
        }
    }
}