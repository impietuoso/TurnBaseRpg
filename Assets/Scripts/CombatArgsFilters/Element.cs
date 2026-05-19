using System;
using Drafts;
using UnityEngine;
using UnityEngine.Scripting;

namespace CombatArgsFilters {
    [Preserve, Serializable]
    public class Element : ICombatArgsFilter, ISingleLineDrawer {
        [SerializeField] private global::Element element;
        public bool Match(CombatArgs args) => args.element == element;
    }
}