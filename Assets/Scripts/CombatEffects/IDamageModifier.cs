using System;
using UnityEngine.Scripting;

public interface IDamageModifier { }

namespace CombatEffects {
    [Preserve, Serializable]
    public class IgnoreShield : ICombatEffect, IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.ignoreShield = true;
    }

    [Preserve, Serializable]
    public class IgnoreArmor : ICombatEffect, IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.ignoreArmor = true;
    }

    [Preserve, Serializable]
    public class Unavoidable : ICombatEffect, IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.unavoidable = true;
    }

    [Preserve, Serializable]
    public class StopReactions : ICombatEffect, IDamageModifier {
        void ICombatEffect.PrepareEffect(CombatArgs args) => args.stopReactionAttacks = true;
    }
}