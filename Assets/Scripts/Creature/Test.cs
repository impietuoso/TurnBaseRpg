using System;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    public class Test : MonoBehaviour
    {
        public CombatController combatController;
        public CreatureSpawn[] startingCreatures;

        private void Start()
        {
            foreach (var c in startingCreatures)
                combatController.Spawn(c.creature, "player", c.position.position);
        }

        [Serializable]
        public class CreatureSpawn
        {
            public PartyMember creature;
            public Transform position;
        }
    }
}