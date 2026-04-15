using System;
using UnityEngine;

public class EffectAttribute : TypeDropdownAttribute {
    public EffectAttribute() : base(typeof(ISkillEffect)) {
        
    }
}