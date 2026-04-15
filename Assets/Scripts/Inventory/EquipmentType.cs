using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/EquipmentType", fileName = "New Equipment Type")]
public class EquipmentType : ScriptableObject {
    public string typeName;
    public Sprite nothingEquipedSprite;
}
