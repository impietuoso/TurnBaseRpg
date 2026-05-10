using UnityEngine;

namespace TTT.ContextMenus {
    public class AttackMenuItem : SkillMenuItem {
        public AttackMenuItem(Character user) : base(user, user.basicAttack[0]) { }
        public override string Title => "Attack";
        public override Sprite Icon => ContextMenuAssets.Instance.AttackIcon;
        public override Color Color => ContextMenuAssets.Instance.AttackColor;
    }
}