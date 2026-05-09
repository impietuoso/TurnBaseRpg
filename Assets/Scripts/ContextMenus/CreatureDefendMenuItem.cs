using System;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class CreatureDefendMenuItem : IContextMenuItem
    {
        public CreatureDefendMenuItem(Character user) => User = user;

        public Character User { get; }
        public string Title => "Defend";
        public Sprite Icon => ContextMenuAssets.Instance.DefendIcon;
        public Color Color => ContextMenuAssets.Instance.DefendColor;
        public bool Enabled => true;
        public void Execute() => throw new NotImplementedException();
    }
}