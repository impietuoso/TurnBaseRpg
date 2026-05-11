using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour {
    public class TargetPickerArgs {
        public TargetPickerArgs(Func<ITarget, bool> validate, Action<ITarget> confirm) {
            Validate = validate;
            Confirm = confirm;
        }

        public ITarget User { get; set; }
        public Func<ITarget, bool> Validate { get; }
        public Action<Character> Confirm { get; }
        public Sprite Icon { get; set; }
        public Color ArrowColor { get; set; }
    }

    public interface IAction {
        float Range { get; }
        float GetChargeTime(Character user);
        IEnumerator Execute(ActionArgs args);
    }

    public class ActionArgs {
        public ActionArgs(IAction action, Character user, ITarget target) {
            Action = action;
            User = user;
            Target = target;
            ChargeTime = action.GetChargeTime(user);
        }

        public IAction Action { get; }
        public Character User { get; }
        public ITarget Target { get; }
        public float ChargeTime { get; }
        public float Timer { get; private set; }
        public HashSet<object> Flags { get; } = new ();

        public float Progress => Timer / ChargeTime;
        public bool Ready => Timer >= ChargeTime;

        public void Charge(float amt) => Timer = Math.Min(Timer + amt, ChargeTime);
        public IEnumerator Execute() => Action.Execute(this);
    }
}