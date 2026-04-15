using System;
using System.Collections.Generic;

[Serializable]
public class StatusRemoval : ISkillEffect {
    public StatusType statusType;
    public bool removeAll;
    public StatusSO removedStatus;

    public void Prepare(CombatArgs args) {
        var statusList = args.target.StatusEffectList.StatusList;
        if (removeAll) {
            var toRemove = new List<StatusSO>();

            foreach (var kvp in statusList) {
                if (kvp.Key.statusType == statusType) {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var so in toRemove) {
                args.target.StatusEffectList.Remove(so);
            } 
        } else {
            var so = new StatusSO();
            foreach (var kvp in statusList) {
                if (kvp.Key.status == removedStatus.status) {
                    so = kvp.Key;
                }
            }
            if(so.status != null) args.target.StatusEffectList.Remove(so);
        }
        
    }
}
