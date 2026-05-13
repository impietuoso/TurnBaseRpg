using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CombatEffects {
    [Preserve, Serializable]
    public class Damage : ICombatEffect {
        public Element element;
        public int damage;
        public StatScale stat;
        [Range(0, 100)] public int hitChance = 80;
        [Range(0, 100)] public int critChance = 10;
        [Range(0, 100)] public float damageRange = 0.15f;

        void ICombatEffect.PrepareEffect(CombatArgs args) {
            var rangedDamage = UnityEngine.Random.Range(1 - damageRange, 1 + damageRange);
            var damageStatValue = args.user[stat.stat] * stat.scale;
            var finalDamage = Mathf.RoundToInt((damage + damageStatValue) * rangedDamage);

            args.damage = finalDamage;
            args.critChance = critChance;
            args.hitChance = hitChance;

            if (element) args.element = element;
        }
    }
}