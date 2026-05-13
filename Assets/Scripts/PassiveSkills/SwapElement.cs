public class SwapElement : IPassive {
    public Element originalElement;
    public Element newElement;
    public void Subscribe(Character character) {
        character.OnAttack += SwapElement;
    }

    public void Unsubscribe(Character character) {
        character.OnAttack -= SwapElement;
    }

    public void SwapElement(CombatArgs args) {
        if (args.skillElement  && args.skillElement == originalElement) {
            args.skillElement = newElement;
        }
    }
}