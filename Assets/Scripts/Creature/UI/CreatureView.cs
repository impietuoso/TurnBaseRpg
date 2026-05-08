using System.Linq;
using Drafts;
using TMPro;
using UnityEngine;

namespace TricksAndTreatsOrThreats.UI
{
    public class CreatureView : DataView<Creature>
    {
        [SerializeField] private DatabaseItemView species;
        [SerializeField] private RaceView race;
        [SerializeField] private CollectionView stats;
        [SerializeField] private CollectionView activities;
        [SerializeField] private TMP_Text displayName;

        protected override void Subscribe()
        {
            var activityPairs = Data.activities.Select(a => ((IScore)a, a.Key.Icons));

            species.TrySetData(Data.race.Species);
            race.TrySetData(Data.race);
            stats.TrySetData(Data.stats);
            activities.TrySetData(activityPairs);
            displayName.TrySetText(Data.DisplayName);
        }

        protected override void Unsubscribe()
        {
            throw new System.NotImplementedException();
        }
    }
}