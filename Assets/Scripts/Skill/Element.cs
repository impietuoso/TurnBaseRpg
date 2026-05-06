using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable/Element", fileName = "New Element")]
public class Element : ScriptableObject  {
    public string elementName;
    public Sprite elementSprite;
    public Color elementColor;
    public List<Element> weak;
}