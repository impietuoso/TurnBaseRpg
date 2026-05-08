using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TricksAndTreatsOrThreats.UI
{
    public class DatabaseItemIconReplicator : DataView<DatabaseItem>
    {
        [SerializeField] private List<Image> icons;

        protected override void Subscribe()
        {
            foreach (var icon in icons)
                icon.overrideSprite = Data.Icon;
        }

        protected override void Unsubscribe()
        {
            foreach (var icon in icons)
                icon.overrideSprite = null;
        }
    }
}