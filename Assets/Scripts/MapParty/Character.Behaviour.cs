using System.Collections.Generic;
using TTT.ContextMenus;
using UnityEngine;

public partial class Character : MonoBehaviour, ITarget
{
    public static HashSet<Character> Instances { get; } = new();

    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    private CharacterCombatMenu _menu;

    public Vector3 Position => transform.position;
    public Vector3 Center => transform.position + Vector3.up;
    public bool IsValid => this;
    public CharacterCombatMenu Menu => _menu ??= new(this, new());

    private void OnEnable() => Instances.Add(this);
    private void OnDisable() => Instances.Remove(this);

    private void Start()
    {
        CreateActionsArgs();
    }

    private void UpdateBehaviour()
    {
        SpriteRenderer.sprite = characterSprite;
    }

    private void Update()
    {
        HandleAction();
    }
}