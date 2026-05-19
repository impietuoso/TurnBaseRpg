using System;
using Drafts;
using UnityEngine;
using UnityEngine.Scripting;

namespace CombatEffects {
    [Preserve, Serializable]
    public class Damage : ICombatEffect {
        public Element element;
        public int damage = 10;
        public StatScale statScale;
        [Label("Hit%"), Range(0, 100)] public int hitChance = 80;
        [Label("Crit%"),Range(0, 100)] public int critChance = 10;
        [Label("Dmg~"),Range(0, 100)] public float damageRange = 0.15f;

        [SerializeReference, TypeInstance] public IDamageModifier[] modifiers;  

        void ICombatEffect.PrepareEffect(CombatArgs args) {
            var rangedDamage = UnityEngine.Random.Range(1 - damageRange, 1 + damageRange);
            var damageStatValue = args.user[statScale.stat] * statScale.scale;
            var finalDamage = Mathf.RoundToInt((damage + damageStatValue) * rangedDamage);

            if (element) args.element = element;
            args.damage = finalDamage;
            args.critChance = critChance;
            args.hitChance = hitChance;

            foreach (var mod in modifiers) 
                mod.PrepareArgs(args);
        }
    }
}