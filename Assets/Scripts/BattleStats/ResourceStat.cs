using System;
using UnityEngine;

[Serializable]
public class ResourceStat {
    public delegate void OnChangedHandle(ResourceStat stat, int delta);

    [SerializeField] private int _max;
    [SerializeField] private int _current;
    public float Normalized => Current / (float)Max;
    public event OnChangedHandle OnChanged;
    public event OnChangedHandle OnMaxChanged;

    public int Max {
        get => _max;
        set {
            var delta = value - _max;
            if (delta == 0) return;
            _max = value;
            if (value < Current) Current = value;
            OnMaxChanged?.Invoke(this, delta);
        }
    }

    public int Current {
        get => _current;
        set {
            value = Math.Clamp(value, 0, _max);
            var delta = value - _current;
            if (delta == 0) return;
            _current = value;
            OnChanged?.Invoke(this, delta);
        }
    }
}