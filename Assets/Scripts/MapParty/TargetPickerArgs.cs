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
        IEnumerator Execute(ActionArgs args);
    }

    public class ActionArgs {
        public ActionArgs(IAction action, Character user, ITarget target) {
            Action = action;
            User = user;
            Target = target;
        }

        public IAction Action;
        public Character User;
        public ITarget Target;
        
        public HashSet<object> Flags { get; } = new ();

        public IEnumerator Execute() => Action.Execute(this);
    }
}