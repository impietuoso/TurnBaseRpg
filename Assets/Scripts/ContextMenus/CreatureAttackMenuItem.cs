using System;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class CreatureAttackMenuItem : IContextMenuItem
    {
        public CreatureAttackMenuItem(Character user) => User = user;

        public Character User { get; }
        public string Title => "Attack";
        public Sprite Icon => ContextMenuAssets.Instance.AttackIcon;
        public Color Color => ContextMenuAssets.Instance.AttackColor;
        public bool Enabled => !User.StatusEffectList.Contain<StunStatus>();
        public void Execute() => throw new NotImplementedException();
    }
}