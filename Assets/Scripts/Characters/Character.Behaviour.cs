using TricksAndTreatsOrThreats.Behaviour;
using TTT.ContextMenus;
using UnityEngine;

public partial class Character : MonoBehaviour, ITarget {
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    private CharacterCombatMenu _menu;
    private Billboarding _billboarding;

    public bool IsVisible => _billboarding.IsVisible;
    public Vector3 Position => transform.position;
    public Vector3 Center => transform.position + Vector3.up;
    public bool IsValid => this;
    public ListInventory<Consumable> Pouch { get; set; }
    public CharacterCombatMenu Menu => _menu ??= new (this, Pouch);

    private void Start() {
        CreateActionsArgs();
        _billboarding = GetComponentInChildren<Billboarding>();
    }

    private void Update() {
        HandleAction();
    }
}