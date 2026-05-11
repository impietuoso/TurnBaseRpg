using System.Collections.Generic;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour {
    public class FormationController : MonoBehaviour {
        [SerializeField] private float radius = 1f;

        public void MoveTo(IReadOnlyList<Character> party, Vector3 position) {
            for (var i = 0; i < party.Count; i++) {
                var angle = i * Mathf.PI * 2 / party.Count;
                var offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                var p = position + offset;
                party[i].MoveTo(p);
            }
        }
    }
}