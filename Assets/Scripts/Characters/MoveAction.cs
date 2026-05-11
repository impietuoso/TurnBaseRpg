using System.Collections;
using TricksAndTreatsOrThreats.Behaviour;

public class MoveAction : IAction {
    public float Range => 0.01f;
    public float GetChargeTime(Character user) => 0;
    public IEnumerator Execute(ActionArgs args) {
        yield break;
    }
}