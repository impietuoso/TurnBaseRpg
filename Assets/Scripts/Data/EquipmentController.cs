public static class EquipmentController {
    public static void EquipItem(LoadSave load, PartyMember member, Equipment newEquipment, int slot) {
        var equipSlot = Game.Config.equipmentOrder[slot];
        if (newEquipment && newEquipment.EquipSlot != equipSlot) return;

        if (member.equips[slot]) load.save.inventory.Add(member.equips[slot], 1);
        if (newEquipment) load.save.inventory.Remove(newEquipment, 1);

        member.equips[slot] = newEquipment;
    }
}