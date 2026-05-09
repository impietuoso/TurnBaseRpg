using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable/GameConfig", fileName = "New Game Config")]
public class GameConfig : ScriptableObject {
    public static GameConfig Instance => instance ??= Resources.Load<GameConfig>("GameConfig");
    private static GameConfig instance;
    [FormerlySerializedAs("equipmentOrder")]
    public EquipmentType[] equipmentArrayOrder;
    public int[] equipmentDrawOrder;

}
