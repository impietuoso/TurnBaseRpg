using System;
using System.Linq;
using Drafts;
using UnityEngine;
using Random = UnityEngine.Random;

//a b c d
//ab ac ad bc bd cd
//abc abd acd bcd
//abcd
namespace TricksAndTreatsOrThreats
{
    [CreateAssetMenu(menuName = "TTT/Breeding")]
    public class Breeding : ScriptableObject
    {
        public Creature Breed(Creature a, Creature b, BreedArgs args, BreedEnv env)
        {
            var species = a.race.Species;
            var race = DRandom.From(a.race, b.race);
            var stats = species.Stats.Select(max => new ScoreT<BonusStat>(max.Key,
                RollStat(args, a.stats[max.Key], b.stats[max.Key], max.Score)));

            return new Creature
            {
                race = race,
                stats = new(stats),
                //activities = a.activities.Union(b.activities).ToList(),
            };
        }

        private int RollStat(BreedArgs args, float x, float y, float max)
        {
            var r = Random.value < args.maxChance
                ? Mathf.Max(x, y)
                : Mathf.LerpUnclamped(x, y, Mathf.Round(Random.value));

            if (Random.value < args.upgradeChance)
                r *= args.upgradeMult;

            return Mathf.RoundToInt(Mathf.Min(max, r));
        }
    }

    [Serializable]
    public class BreedEnv { }

    [Serializable]
    public class BreedArgs
    {
        public float maxChance = .5f;
        public float upgradeChance = .25f;
        public float upgradeMult = 1.15f;
    }
}