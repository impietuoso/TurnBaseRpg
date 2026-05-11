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
                combatController.Spawn(c.creature, true, c.position.position);
            
            foreach (var c in startingEnemies)
                combatController.Spawn(c.creature, false, c.position.position);
        }

        [Serializable]
        public class CreatureSpawn
        {
            public PartyMember creature;
            public Transform position;
        }
    }
}