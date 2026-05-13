using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GameConfig", fileName = "New Game Config")]
public class GameConfig : ScriptableObject {
    public EquipSlot[] equipmentOrder;
    public int[] equipmentDrawOrder;
}
