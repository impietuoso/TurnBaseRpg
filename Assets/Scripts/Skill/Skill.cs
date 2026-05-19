using System.Collections;
using TricksAndTreatsOrThreats;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skill", fileName = "New Skill")]
public class Skill : DatabaseItem, IAction {
    [SerializeField] private string displayName;
    [TextArea(3, 6)] public string skillDescription;
    [field: SerializeField] public float Range { get; private set; } = 5;
    [SerializeField] private int cost;
    public Element element;

    [Header("Config")]
    [SerializeReference, TypeDropdown] public ISkillAnimation animation;
    [SerializeReference, TypeDropdown] public ICombatEffect[] skillEffects;

    public int Cost => cost;
    public override string DisplayName => displayName;

    public bool Available(Character user) {
        if (user.Mana.Current < Cost) return false;
        if (user.StatusEffectList.Contain<Silence>()) return false;

        foreach (var c in user.CombatController.Characters) {
            if (!c.IsVisible) continue;
            if (animation.ValidateTarget(user, c)) return true;
        }

        return false;
    }

    public float GetChargeTime(Character user) => user.Stats.Speed.Total / 10f;

    public IEnumerator Execute(ActionArgs args) {
        if (args.User) args.User.Mana.Current -= cost;
        return animation.Play(this, args);
    }
}

public interface ICombatEffect {
    protected void PrepareEffect(CombatArgs args);

    public sealed void PrepareArgs(CombatArgs args) {
        PrepareEffect(args);
        args.Effects.Add(this);
    }
}