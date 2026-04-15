using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectList {
    private Character target;
    private Dictionary<StatusSO, Status> statusList;
    public IReadOnlyDictionary<StatusSO, Status> StatusList => statusList;
    public event Action<Status> OnStatusAdded;
    public event Action<Status> OnStatusRemoved;

    public StatusEffectList(Character target) {
        this.target = target;
        statusList = new Dictionary<StatusSO, Status>();
    }
    
    public void Apply(StatusSO so) {
        if (statusList.TryGetValue(so, out Status existingStatus)) {
            existingStatus.Stack(target, so.status);
        } else {
            Status newStatus = so.Clone();
            if (newStatus != null) {
                statusList.Add(so, newStatus);
                newStatus.Apply(target);
                OnStatusAdded?.Invoke(newStatus);
            }
        }
    }
    
    public void Remove(StatusSO so) {
        if (statusList.TryGetValue(so, out Status status)) {
            status.Remove(target);
            statusList.Remove(so);
            OnStatusRemoved?.Invoke(status);
        }
    }

    public bool Contain(StatusSO newSo) {
        return statusList.ContainsKey(newSo);
    }
}