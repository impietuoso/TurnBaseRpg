using System;
using System.Collections;
using TTT.ContextMenus;
using UnityEngine;

public partial class Character : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    private Coroutine _moveCoroutine;
    private CharacterCombatMenu _menu;
    public CharacterCombatMenu Menu => _menu ??= new(this, new());

    private void UpdateBehaviour()
    {
        SpriteRenderer.sprite = characterSprite;
    }

    public void MoveTo(Vector3 position)
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MoveRoutine(position));
    }

    public void UseSkill(Skill skill, Character target)
    {
        throw new NotImplementedException();
    }

    private IEnumerator MoveRoutine(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) <= 0.01f)
            yield break;

        if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            Animator.Play("Walk");

        var speed = 2;

        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = position;
        Animator.Play("Idle");
        _moveCoroutine = null;
    }
}