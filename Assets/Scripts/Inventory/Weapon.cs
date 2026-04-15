using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Weapon", fileName = "New Weapon")]
public class Weapon : Equipment {
    public bool twoHanded;
    public Skill basicAttack;
}
