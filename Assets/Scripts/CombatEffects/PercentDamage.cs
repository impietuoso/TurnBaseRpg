using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CombatEffects {
    [Preserve, Serializable]
    public class PercentDamage : ICombatEffect {
        public Element element;
        [Range(0f, 1f)] public float percent = 0.25f;
        [Range(0, 100)] public int hitChance = 100;

        void ICombatEffect.PrepareEffect(CombatArgs args) {
            var damage = Mathf.RoundToInt(args.target.Health.Max * percent);
            args.damage = damage;
            args.ignoreArmor = true;
            args.hitChance = hitChance;

            if (element) args.element = element;
        }
    }
}