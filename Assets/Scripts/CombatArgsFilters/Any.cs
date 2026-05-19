namespace CombatArgsFilters {
    public class Any : ICombatArgsFilter {
        public bool Match(CombatArgs args) => true;
    }
}