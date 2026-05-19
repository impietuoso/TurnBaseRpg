using System;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    public class Test : MonoBehaviour
    {
        public CombatController combatController;
        public CreatureSpawn[] startingCreatures;
        public CreatureSpawn[] startingEnemies;

        private void Start()
        {
            foreach (var c in startingCreatures)
                combatController.Spawn(c.so.Creature, true, c.position.position);
            
            foreach (var c in startingEnemies)
                combatController.Spawn(c.so.Creature, false, c.position.position);
        }

        [Serializable]
        public class CreatureSpawn
        {
            public CreatureSO so;
            public Transform position;
        }
    }
}