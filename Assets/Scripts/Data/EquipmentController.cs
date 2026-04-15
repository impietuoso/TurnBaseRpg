using System.Linq;

public static class EquipmentController {

    public static void EquipItem(LoadSave load, PartyMember member, Equipment newEquipment, int slot) {
        if(newEquipment != null && newEquipment.equipmentType != GameConfig.Instance.equipmentArrayOrder[slot]) return;
        
        var hasTwoHandedEquiped = member.equips.Any(e => e is Weapon w && w.twoHanded);
        var wantToEquipWeapon = newEquipment is Weapon;
        var wantToEquipTwoHanded = newEquipment is Weapon w && w.twoHanded;

        if (wantToEquipWeapon && hasTwoHandedEquiped || wantToEquipTwoHanded) {
            for (int i = 0; i < member.equips.Count; i++) {
                Equipment item = member.equips[i];
                if (item is Weapon) {
                    member.equips[i] = null;
                    load.save.inventory.Add(item, 1);
                }
            }
        }
        
        if (member.equips[slot] != null) {
            var oldEquipment = member.equips[slot];
            load.save.inventory.Add(oldEquipment, 1);
        }
        
        if (newEquipment != null)
            load.save.inventory.Remove(newEquipment, 1); 
        
        member.equips[slot] = newEquipment;
    }
}
