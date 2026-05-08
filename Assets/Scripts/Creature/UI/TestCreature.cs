using UnityEngine;

namespace TricksAndTreatsOrThreats.UI
{
    public class TestCreature : MonoBehaviour
    {
        public Race race;
        public Creature creature;
        public AnimationCurve curve;
        [Range(0, 10)] public int testCurve;
        public int maxStat = 25;
        public int curveResult;

        private void OnValidate()
        {
            curveResult = Mathf.RoundToInt(maxStat * curve.Evaluate(testCurve / 10f));

            if (creature!.race == race) return;
            creature.race = race;
            creature.stats = new(race.Stats);
            creature.activities = new(race.Activities);
        }
    }
}