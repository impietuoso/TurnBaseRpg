using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Attribute;
using static Stat;

[Serializable]
public class Stats {
    public static int Count => All.Count;
    public static IReadOnlyList<Stat> All { get; } = Enum.GetValues(typeof(Stat)).OfType<Stat>().ToArray();
    public static IStats Zero { get; } = new StatsBase();

    [SerializeField] private BonusStat[] _stats;

    public Stats() {
        _stats = new BonusStat[Count];
        for (var i = 0; i < _stats.Length; i++)
            _stats[i] = new ();

        _stats[(int)MaxHealth].Max = 9999;
        _stats[(int)MaxShield].Max = 9999;
        _stats[(int)MaxMana].Max = 9999;
        _stats[(int)Evade].Max = 40;
        _stats[(int)Resistance].Max = 60;
    }

    public BonusStat this[Stat stat] => _stats[(int)stat];

    public void Recalculate(int level, IAttributes a, IStats bonus) =>
        new StatCalcHelper(this).Recalculate(level, a, bonus);
}

public class StatCalcHelper : IStats {
    private readonly Stats _context;
    readonly int[] _v = new int[12];
    public StatCalcHelper(Stats context) => _context = context;
    public int this[Stat s] { get => _v[(int)s]; private set => _v[(int)s] = value; }

    public void Recalculate(int level, IAttributes a, IStats stats) {
        this[MaxHealth] = 2 * level + a[Vit] * 15;
        this[MaxMana] = 6 * level + a[Spt] * 8;
        this[MaxShield] = this[MaxHealth] / 2;
        this[Speed] = level + a[Dex] * 4;
        this[Evade] = a[Dex] * 100 / (a[Dex] + 40);
        this[Resistance] = (a[Vit] * 2 + a[Spt]) * 100 / (a[Vit] * 2 + a[Spt] + 60);

        foreach (var s in Stats.All)
            _v[(int)s] += stats[s];

        foreach (var s in Stats.All)
            _context[s].Base = _v[(int)s];
    }
}