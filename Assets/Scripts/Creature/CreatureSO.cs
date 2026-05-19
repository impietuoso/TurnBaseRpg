using UnityEngine;

namespace TricksAndTreatsOrThreats {
    // ReSharper disable once InconsistentNaming
    [CreateAssetMenu(menuName = "Game/CreatureSO")]
    public class CreatureSO : ScriptableObject {
        [SerializeField] private Creature creature;
        public Creature Creature => creature;
    }
}