using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat {
    public Stat(int maxValue) {
        minValue = 0;
        this.maxValue = maxValue;
        baseValue = maxValue;
        currentValue = maxValue;
        bonusValue = new();
    }
    
    public Stat(int baseValue, int maxValue) {
        minValue = 0;
        this.maxValue = maxValue;
        this.baseValue = baseValue;
        currentValue = baseValue;
        bonusValue = new();
    }

    [field:SerializeField] public int minValue { get; private set; }
    [field:SerializeField] public int maxValue { get; private set;}
    [field:SerializeField] public int baseValue { get; private set; }
    [field:SerializeField] public int currentValue { get; private set; }
    public Dictionary<object, float> bonusValue { get; private set; }
    public event Action<int> OnChange;
    
    public void AddMinValue(int value) { minValue += value; Recalculate(); }
    
    public void AddMaxValue(int value) { maxValue += value; Recalculate(); }
    
    public void AddBaseValue(int value) { baseValue += value; Recalculate(); }

    public void AddBonus(object newBonus, float value) { bonusValue[newBonus] = value; Recalculate(); }
    
    public void RemoveBonus(object newBonus) { bonusValue.Remove(newBonus); Recalculate(); }
    
    public void AddClampedBaseValue(int value) {
        baseValue = Mathf.Clamp(baseValue+value, minValue, maxValue);
        Recalculate();
    }
    
    public void Recalculate() {
        float theCoolerCurrentValue = baseValue;
        foreach (var bonus in bonusValue) {
            theCoolerCurrentValue *= bonus.Value;
        }
        currentValue = Mathf.Clamp((int)theCoolerCurrentValue,minValue, maxValue);
        OnChange?.Invoke(currentValue);
    }
}