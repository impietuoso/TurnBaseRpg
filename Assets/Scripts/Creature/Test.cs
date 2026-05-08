using System;
using Drafts;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    public class Test : MonoBehaviour
    {
        [Prefab] public CreatureBehaviour creaturePrefab;
        public CreatureSpawn[] startingCreatures;

        private void Start()
        {
            foreach (var c in startingCreatures)
            {
                var clone = creaturePrefab.Clone(c.creature);
                clone.transform.position = c.position.position;
            }
        }

        private void OnValidate()
        {
            foreach (var p in startingCreatures)
            {
                p.creature.stats = new(p.creature.race.Stats);
                p.creature.activities = new(p.creature.race.Activities);
            }
        }

        [Serializable]
        public class CreatureSpawn
        {
            public Creature creature;
            public Transform position;
        }
    }
}