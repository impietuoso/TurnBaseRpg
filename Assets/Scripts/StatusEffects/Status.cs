using System;

public abstract class Status {
    [NonSerialized]
    public StatusSO source;
    public string statusName;
    public string statusDescription;
    public StatusSO opposite;
    public abstract void Apply(Character target);
    public abstract  void Remove(Character target);
    public abstract  void Stack(Character target, Status other);
    public abstract Observable<int> DisplayValue { get; }

    public bool TryNullifyOpposite(Character target) {
        if (opposite && target.StatusEffectList.Contain(opposite)) {
            target.StatusEffectList.Remove(opposite);
            target.StatusEffectList.Remove(source);
            return true;
        }

        return false;
    }
    
}