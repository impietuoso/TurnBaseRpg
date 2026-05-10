using System.Collections;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

public partial class Character 
{
    [SerializeField] private float stopDistance = .2f;

    private readonly TargetPosition _targetPosition = new();
    private ActionArgs _moveArgs;
    private Coroutine _currentAction;
    
    public ActionArgs NextAction { get; set; }

    private void CreateActionsArgs()
    {
        _moveArgs = new(null, this, _targetPosition);
    }

    public void MoveTo(Vector3 position)
    {
        _targetPosition.Position = position;
        NextAction = _moveArgs;
    }

    private void HandleAction()
    {
        if (_currentAction != null) return;
        if (NextAction == null) return;

        var curr = transform.position;
        var tgt = NextAction.Target.Position;
        var speed = 2 * Time.deltaTime;
        var next = Vector3.MoveTowards(curr, tgt, speed);
        var dist = (next - tgt).sqrMagnitude;
        transform.position = next;

        if (NextAction.Action == null)
        {
            if (dist <= stopDistance)
                NextAction = null;
            return;
        }

        var range = NextAction.Action.Range;
        if (dist > range * range) return;
        _currentAction = StartCoroutine(ExecuteNextAction());
        NextAction = null;
        
        //TODO Animator.Play("Walk"); 
        //TODO Animator.Play("Idle");
    }

    private IEnumerator ExecuteNextAction()
    {
        var ie = NextAction.Execute();
        NextAction = null;
        yield return ie;
        _currentAction = null;
    }
}