using Drafts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TTT.ContextMenus
{
    public class ContextMenuItemView : DataView<IContextMenuItem>
    {
        [field: SerializeField] public ContextMenuView Menu { get; private set; }
        [field: SerializeField] public Image Background { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }
        [field: SerializeField] public UnityEvent<bool> OnEnabled { get; private set; }

        protected override void Subscribe()
        {
            Icon.TrySetSprite(Data.Icon);
            Text.TrySetText(Data.Title);
            Background.TrySetColor(Data.Color);
        }

        protected override void Unsubscribe() { }
        private void Update() => OnEnabled.Invoke(Data.Enabled);

        public void Execute()
        {
            if (Data is IContextMenu next) Menu.Next(next);
            else if (Data is BackMenuItem) Menu.Back();
            else
            {
                Data.Execute();
                Menu.gameObject.SetActive(false);
            }
        }
    }
}