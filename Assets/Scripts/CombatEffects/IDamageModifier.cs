using System;
using Drafts;
using UnityEngine;
using UnityEngine.Scripting;

public interface IDamageModifier : ICombatEffect { }

namespace CombatEffects {
    [Preserve, Serializable]
    public class IgnoreShield : IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.ignoreShield = true;
    }

    [Preserve, Serializable]
    public class IgnoreArmor : IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.ignoreArmor = true;
    }

    [Preserve, Serializable]
    public class Unavoidable : IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.unavoidable = true;
    }

    [Preserve, Serializable]
    public class StopReactions : IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.stopReactionAttacks = true;
    }

    [Preserve, Serializable]
    public class LifeSteal : IDamageModifier, ISingleLineDrawer {
        [Range(0f, 1f)] public float percent = 0.2f;

        void ICombatEffect.PrepareEffect(CombatArgs args) {
            args.OnResolve += Steal;
        }

        public void Steal(CombatArgs args) {
            var heal = args.result.Health.Delta * percent;
            if (heal <= 0) return;

            var chain = args.Chain(this, args.user);
            chain.heal = (int)heal;
            chain.Resolve();
        }
    }
}