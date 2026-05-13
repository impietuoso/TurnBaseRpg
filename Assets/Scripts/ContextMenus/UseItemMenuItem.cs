using System.Collections;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

namespace TTT.ContextMenus {
    public class UseItemMenuItem : IContextMenuItem, IAction {
        public UseItemMenuItem(Character user, ListInventory<Consumable> pouch, Slot<Consumable> slot) {
            User = user;
            Pouch = pouch;
            Slot = slot;

            if (Slot.item.skillEffect.animation.NeedTarget)
                PickerArgs = new (Validate, Enqueue)
                {
                    User = user,
                    Icon = slot.item.Icon,
                    ArrowColor = Color.green,
                };
        }

        public Character User { get; }
        public ListInventory<Consumable> Pouch { get; }
        public Slot<Consumable> Slot { get; }
        public TargetPickerArgs PickerArgs { get; }
        public string Title => Slot.item.displayName + " x" + Slot.amount;
        public Sprite Icon => Slot.item.Icon;
        public Color Color => ContextMenuAssets.Instance.ItemColor;
        public bool Enabled => Slot.amount > 0;
        public float Range => 3f;

        public void Enqueue(ITarget tgt) => User.NextAction = new (this, User, tgt);
        public bool Validate(ITarget tgt) => Slot.item.skillEffect.animation.ValidateTarget(User, tgt);

        public void Execute() {
            if (Slot.item.skillEffect.animation.NeedTarget)
                TargetPicker.Instance.ShowArrow(PickerArgs);
            else Enqueue(User);
        }

        public float GetChargeTime(Character user) => 1;

        IEnumerator IAction.Execute(ActionArgs args) {
            Pouch.Remove(Slot.item, 1);
            yield return Slot.item.skillEffect.Execute(args);
        }
    }
}