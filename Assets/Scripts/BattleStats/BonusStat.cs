using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BonusStat {
    [SerializeField] private int min, max = 999;
    [SerializeField] private int @base, total;

    private readonly Dictionary<object, float> _bonus = new ();

    public int Total { get; private set; }
    public float Normalized => Total / (float)Max;
    public event Action<int> OnChanged;
    public static implicit operator int(BonusStat stat) => stat.Total;

    public int Min {
        get => min;
        set {
            min = value;
            Recalculate();
        }
    }

    public int Max {
        get => max;
        set {
            max = value;
            Recalculate();
        }
    }

    public int Base {
        get => @base;
        set {
            @base = value;
            Recalculate();
        }
    }

    public void AddBonus(object key, float value) {
        _bonus[key] = value;
        Recalculate();
    }

    public void RemoveBonus(object key) {
        if (_bonus.Remove(key))
            Recalculate();
    }

    public void Recalculate() {
        float v = Base;
        foreach (var bonus in _bonus)
            v *= bonus.Value;

        Total = Mathf.Clamp((int)v, Min, Max);
        OnChanged?.Invoke(Total);
    }
}