using Drafts;
using TMPro;
using UnityEngine;

namespace TricksAndTreatsOrThreats.UI {
    public class CreatureView : DataView<Creature> {
        [SerializeField] private TMP_Text displayName;
        [SerializeField] private TMP_Text level;
        [SerializeField] private DatabaseItemView species;
        [SerializeField] private RaceView race;
        [SerializeField] private IStatsView stats;
        [SerializeField] private CollectionView skills;
        [SerializeField] private CollectionView equips;
        [SerializeField] private CollectionView activities;

        protected override void Subscribe() {
            displayName.TrySetText(Data.DisplayName);
            level.TrySetText(Data.level);
            species.TrySetData(Data.Race.Species);
            race.TrySetData(Data.Race);
            stats.TrySetData(Data);
            skills.TrySetData(Data.Skills);
            equips.TrySetData(Data.Equips);
        }

        protected override void Unsubscribe() { }
    }
}