using UnityEngine;

namespace TTT.ContextMenus {
    public class DefendMenuItem : SkillMenuItem {
        public DefendMenuItem(Character user) : base(user, user.CombatController.BasicDefendSkill) { }
        public override string Title => "Defend";
        public override Sprite Icon => ContextMenuAssets.Instance.DefendIcon;
        public override Color Color => ContextMenuAssets.Instance.DefendColor;
    }
}