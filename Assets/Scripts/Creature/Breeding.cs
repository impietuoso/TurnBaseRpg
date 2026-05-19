using System;
using System.Linq;
using Drafts;
using UnityEngine;
using Random = UnityEngine.Random;

//a b c d
//ab ac ad bc bd cd
//abc abd acd bcd
//abcd
namespace TricksAndTreatsOrThreats {
    [CreateAssetMenu(menuName = "TTT/Breeding")]
    public class Breeding : ScriptableObject {
        public Creature Breed(Creature a, Creature b, BreedArgs args, BreedEnv env) {
            var species = a.Race.Species;
            var race = DRandom.From(a.Race, b.Race);
            var attributes = new Attributes();

            foreach (var attr in Attributes.All)
                attributes[attr] = RollStat(args,
                    a.BornAttributes[attr],
                    b.BornAttributes[attr],
                    species.Attributes[attr]);

            return new Creature(race, attributes);
        }

        private int RollStat(BreedArgs args, int x, int y, int max) {
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
    public class BreedArgs {
        public float maxChance = .5f;
        public float upgradeChance = .25f;
        public float upgradeMult = 1.15f;
    }
}