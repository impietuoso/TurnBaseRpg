using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class CombatTask : MonoBehaviour
    {
        private void OnEnable() => CombatController.Instance.TaskList.Add(this);
        private void OnDisable() => CombatController.Instance.TaskList.Remove(this);
    }
}