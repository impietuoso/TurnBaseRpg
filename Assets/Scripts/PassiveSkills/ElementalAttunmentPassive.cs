using System;
using UnityEngine.Scripting;

[Preserve, Serializable]
public class ElementalAttunmentPassive : IPassive {
    public Element element;
    public void Subscribe(Character character) => character.element = element;
    public void Unsubscribe(Character character) => character.element = character.Member.element;
}