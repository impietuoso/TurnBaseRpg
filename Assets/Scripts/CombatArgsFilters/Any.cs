using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CombatArgsFilters {
    public class Any : ICombatArgsFilter {
        public bool Match(CombatArgs args) => true;
    }

    [Preserve, Serializable, SingleLine]
    public class Element : ICombatArgsFilter {
        [SerializeField] private global::Element element;
        public bool Match(CombatArgs args) => args.element == element;
    }
}