using Drafts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TricksAndTreatsOrThreats.UI
{
    public class DatabaseItemView : DataView<DatabaseItem>
    {
        [SerializeField] private TMP_Text displayName;
        [SerializeField] private Image icon;

        protected override void Subscribe()
        {
            displayName.TrySetText(Data.DisplayName);
            icon.TrySetSprite(Data.Icon);
        }

        protected override void Unsubscribe()
        {
            throw new System.NotImplementedException();
        }
    }
}