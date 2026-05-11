using System;
using UnityEngine.Scripting;

[Preserve, Serializable]
public class ElementalAttunmentPassive : IPassiveSkill {
    public Element element;
    public void Subscribe(Character character) => character.element = element;
    public void Unsubscribe(Character character) => character.element = character.Member.element;
}