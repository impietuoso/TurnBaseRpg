using System.Collections.Generic;
using Drafts;
using UnityEngine;

namespace TTT.ContextMenus
{
    public class ContextMenuView : DataView<IContextMenu>
    {
        [field: SerializeField] public CollectionView Items { get; private set; }
        private Stack<IContextMenu> Stack { get; } = new();

        protected override void Subscribe()
        {
            Stack.Clear();
            Stack.Push(Data);
            Items.TrySetData(Data.GetItems());
        }

        protected override void Unsubscribe()
        {
            Stack.Clear();
            Items.SetData(null);
        }

        public void Next(IContextMenu next)
        {
            Items.SetData(next.GetItems());
            Stack.Push(next);
        }

        public void Back()
        {
            if (Stack.Count == 1) return;
            Stack.Pop();
            Items.SetData(Stack.Peek().GetItems());
        }
    }
}