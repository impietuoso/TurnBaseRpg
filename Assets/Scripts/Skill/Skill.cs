using System.Collections;
using System.Linq;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skill", fileName = "New Skill")]
public class Skill : ScriptableObject, IAction {
    [Header("Ui")]
    public string skillName;
    [TextArea(3, 6)]
    public string skillDescription;
    public int cost;
    public Sprite icon;
    [Header("Config")]
    public Element element;
    [SerializeReference, TypeDropdown(typeof(ISkillAnimation))]
    public ISkillAnimation animation;
    [SerializeReference, Effect]
    public ISkillEffect[] skillEffects;
    [SerializeReference, TypeDropdown(typeof(IPassiveSkill))] public IPassiveSkill passiva;

    public float Range => 10;

    public bool Available(Character user) {
        foreach (var status in user.StatusEffectList.StatusList) {
            if (status.Value is Silence) return false;
        }
        
        if (user.derivedStats.mana.currentValue < cost) return false;

        if (CombatManager.instance.combatUI.characters.All
                (c => !animation.ValidateTarget(user, c.owner))) return false;

        return true;
    }

    public IEnumerator Execute(Character user, ITarget target)
    {
        return animation.Play(this, user, target);
    }

    public virtual IEnumerator UseSkill(Character user, Character target, CombatManager combatManager) {
        Debug.Log(user.characterName + " used " + skillName);
        return animation.Play(this, user, target, combatManager);
    }
}

public interface ISkillEffect {
    public void Prepare(CombatArgs args);
}