using System.Collections.Generic;
using UnityEngine;

namespace TTT.ContextMenus
{
    public interface IContextMenu : IContextMenuItem
    {
        void IContextMenuItem.Execute() { }
        IEnumerable<IContextMenuItem> GetItems();
    }

    public interface IContextMenuItem
    {
        Sprite Icon { get; }
        Color Color { get; }
        string Title { get; }
        bool Enabled { get; }
        void Execute();
    }

    public interface ITargetingMenuItem
    {
        Color ArrowColor { get; }
        bool ValidateTarget(Character target);
    }

    public class BackMenuItem : IContextMenuItem
    {
        public static BackMenuItem Item { get; } = new();
        public string Title => "Back";
        public Sprite Icon => ContextMenuAssets.Instance.BackIcon;
        public Color Color => ContextMenuAssets.Instance.BackColor;
        public bool Enabled => true;
        public void Execute() { }
    }
}