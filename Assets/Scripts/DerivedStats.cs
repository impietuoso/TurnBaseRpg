using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DerivedStats {
    [field:SerializeField] public Stat damage { get; private set; }
    [field:SerializeField] public Stat health { get; private set; }
    [field:SerializeField] public Stat shield { get; private set; }
    [field:SerializeField] public Stat mana { get; private set; }
    [field:SerializeField] public Stat speed { get; private set; }
    [field:SerializeField] public Stat armor { get; private set; }
    [field:SerializeField] public Stat resistance { get; private set; }
    [field:SerializeField] public Stat evade { get; private set; }
    
    public void CalculateDeviredStats(BaseStats stats, Profession profession, IEnumerable<Equipment> equips, int level) {
        BaseStats s = stats + profession.initialStats;
        damage = new Stat(0,9999);
        health = new (2 * level + s.vitality * 15);
        mana = new (6 * level + s.spirit * 8);
        shield = new (health.maxValue/2);
        shield.AddBaseValue(-shield.currentValue);
        speed = new (level + s.dexterity * 4, 999);
        armor = new Stat(0, 100);
        resistance = new((s.vitality * 2 + s.spirit) * 100 / (s.vitality * 2 + s.spirit + 60), 60);
        evade = new(s.dexterity * 100 / (s.dexterity + 40), 40);

        if (equips != null) {
            foreach (var newEquipment in equips) {
                if (newEquipment == null) {
                    continue;
                }
                AddBase(newEquipment.bonusValue);
            }
        }
        
        health.AddClampedBaseValue(health.maxValue);
        mana.AddClampedBaseValue(mana.maxValue);
    }

    public void AddBase(DerivedStatsBase bonusStats) {
        damage.AddBaseValue(bonusStats.damage);
        health.AddMaxValue(bonusStats.health);
        mana.AddMaxValue(bonusStats.mana);
        shield.AddBaseValue(bonusStats.shield);
        speed.AddBaseValue(bonusStats.speed);
        armor.AddBaseValue(bonusStats.armor);
        resistance.AddBaseValue(bonusStats.resistance);
        evade.AddBaseValue(bonusStats.evade);
    }
}
