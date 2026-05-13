public class SwapElement : IPassive {
    public Element originalElement;
    public Element newElement;

    public void Subscribe(Character character) {
        character.OnAttack += Swap;
    }

    public void Unsubscribe(Character character) {
        character.OnAttack -= Swap;
    }

    private void Swap(CombatArgs args) {
        if (args.element && args.element == originalElement) {
            args.element = newElement;
        }
    }
}