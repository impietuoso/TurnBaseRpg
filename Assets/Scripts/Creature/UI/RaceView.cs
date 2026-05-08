using Drafts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TricksAndTreatsOrThreats.UI
{
    public class RaceView : DataView<Race>
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
            displayName.TrySetText("---");
            icon.TrySetSprite(null);
        }
    }
}