using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterFeedbacks : DataView<Character> {
    [SerializeField] private SpriteRenderer characterSprite;
    // [SerializeField] private ResourceStatView healthView;
    // [SerializeField] private ResourceStatView shieldView;
    [SerializeField] private StatusEffectListView statusEffectView;

    private void Start() => SetData(GetComponent<Character>());

    protected override void Subscribe() {
        // if (healthView) healthView.SetStat(Data.Health);
        // if (shieldView) shieldView.SetStat(Data.Stats.Shield);
        if (statusEffectView) statusEffectView.SetData(Data.StatusEffectList);

        Data.StatusEffectList.OnStatusAdded += HandleNewStat;
        Data.OnResolveDefend += HandleHealthChanged;
    }

    protected override void Unsubscribe() {
        Data.StatusEffectList.OnStatusAdded -= HandleNewStat;
        Data.OnResolveDefend -= HandleHealthChanged;
    }

    private void HandleHealthChanged(CombatArgs args) {
        var damageColor = args.result switch {
            { Miss: true } => Color.white,
            { ResistStatus: true } => Color.orange,
            { Crit: true } => Color.yellow,
            { Health: { Delta: > 0 } } => Color.green,
            { Health: { Delta: < 0 } } => Color.red,
            { Mana: { Delta: > 0 } } => Color.blue,
            { Mana: { Delta: < 0 } } => Color.blueViolet,
            { Shield: { Delta: > 0 } } => Color.cyan,
            { Shield: { Delta: < 0 } } => Color.gray3,
            _ => Color.deepPink
        };

        var statusApplied = args.statusEffects.Count > 0 && !args.result.ResistStatus;
        if (statusApplied && args.damage == 0) return;

        var popupValue = args.result.Shield.Delta + args.result.Health.Delta;
        if (args.user == args.target && popupValue == 0) return;

        var popupText = args.result.Miss ? "Miss" : popupValue.ToString();
        var popup = Data.CombatController.CallPopup;

        if (!args.result.Miss && args.result.ResistStatus) {
            if (popupValue == 0)
                popupText = "Resist";
            else
                popupText += "\nResist";
        }

        if (characterSprite) StartCoroutine(popup.Pop(popupText, damageColor, characterSprite.transform));
        if (args.result.IsFatal && characterSprite) characterSprite.color = new Color(1, 1, 1, 0.5f);
        if (args.result.IsRevive && characterSprite) characterSprite.color = new Color(1, 1, 1, 1f);
    }

    private void HandleNewStat(Status status) {
        var popup = Data.CombatController.CallPopup;
        var color = status.source.statusPopupColor;
        StartCoroutine(popup.Pop(status.statusName, color, characterSprite.transform));
    }
}