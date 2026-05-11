using System.Collections;
using TricksAndTreatsOrThreats;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skill", fileName = "New Skill")]
public class Skill : DatabaseItem, IAction {
    public string skillName;
    [TextArea(3, 6)]
    public string skillDescription;
    [field: SerializeField] public float Range { get; private set; } = 10;
    [SerializeField] private int cost;
    public Element element;

    [Header("Config")]
    [SerializeReference, TypeDropdown] public ISkillAnimation animation;
    [SerializeReference, TypeDropdown] public ISkillEffect[] skillEffects;
    [SerializeReference, TypeDropdown] public IPassiveSkill passiva;

    public int Cost => cost;

    public bool Available(Character user) {
        if (user.derivedStats.mana.currentValue < Cost) return false;
        if (user.StatusEffectList.Contain<Silence>()) return false;

        foreach (var c in user.CombatController.Characters) {
            if (!c.IsVisible) continue;
            if (animation.ValidateTarget(user, c)) return true;
        }

        return false;
    }

    public float GetChargeTime(Character user) => user.derivedStats.speed.currentValue / 10f;

    public IEnumerator Execute(ActionArgs args) {
        args.User?.derivedStats.mana.AddClampedBaseValue(-cost);
        return animation.Play(this, args);
    }
}

public interface ISkillEffect {
    public void Prepare(CombatArgs args);
}