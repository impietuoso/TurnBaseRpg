using System.Collections;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

public partial class Character {
    private static readonly MoveAction MoveAction = new ();

    private readonly TargetPosition _targetPosition = new ();
    private ActionArgs _moveArgs;
    private Coroutine _currentAction;

    public ActionArgs NextAction { get; set; }
    public bool InAction => _currentAction != null;

    private void CreateActionsArgs() {
        _moveArgs = new (MoveAction, this, _targetPosition);
    }

    public void MoveTo(Vector3 position) {
        _targetPosition.Position = position;
        NextAction = _moveArgs;
    }

    private void HandleAction() {
        if (InAction) return;
        if (NextAction == null) return;

        var curr = transform.position;
        var tgt = NextAction.Target.Position;
        var speed = 2 * Time.deltaTime;
        var range = NextAction.Action.Range;
        var dist = (curr - tgt).sqrMagnitude;

        if (dist > range * range) {
            var next = Vector3.MoveTowards(curr, tgt, speed);
            transform.position = next;
            return;
        }

        if (!NextAction.Ready) return;
        _currentAction = StartCoroutine(ExecuteNextAction());

        //TODO Animator.Play("Walk"); 
        //TODO Animator.Play("Idle");
    }

    private IEnumerator ExecuteNextAction() {
        var ie = NextAction?.Execute();
        NextAction = null;
        yield return ie;
        _currentAction = null;
    }
}