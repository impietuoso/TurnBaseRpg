using UnityEngine;
public abstract class Item : ScriptableObject, IItem {
    [field : SerializeField] public string displayName { private set; get; }
    [field : SerializeField] public string description { private set; get; }
    [field : SerializeField] public Sprite sprite { private set; get; }
    [field: SerializeField] public int maxStack { private set; get; } = 99;

}