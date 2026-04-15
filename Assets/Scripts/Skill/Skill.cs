using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skill", fileName = "New Skill")]
public class Skill : ScriptableObject {
    [Header("Ui")]
    public string skillName;
    [TextArea(3, 6)]
    public string skillDescription;
    public int cost;
    public Sprite icon;
    [Header("Config")]
    [SerializeReference, TypeDropdown(typeof(ISkillAnimation))]
    public ISkillAnimation animation;
    [SerializeReference, Effect]
    public ISkillEffect[] skillEffects;
    
    public bool Available(Character user) {
        foreach (var status in user.StatusEffectList.StatusList) {
            if (status.Value is Silence) return false;
        }
        
        if (user.derivedStats.mana.currentValue < cost) return false;

        if (CombatManager.instance.combatUI.characters.All
                (c => !animation.ValidateTarget(user, c.owner))) return false;

        return true;
    }
    
    public virtual void UseSkill(Character user, Character target, CombatManager combatManager) {
        Debug.Log(user.characterName + " used " + skillName);
        combatManager.StartCoroutine(ResetTargetSelectionsFromUI(combatManager, animation.Play(this, user, target, combatManager)));
    }

    public IEnumerator ResetTargetSelectionsFromUI(CombatManager combatManager, IEnumerator skillUsage) {
        yield return skillUsage;
        combatManager.combatUI.ResetSelections();
    }

}

public interface ISkillEffect {
    public void Prepare(CombatArgs args);
}