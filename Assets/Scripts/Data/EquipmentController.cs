using TricksAndTreatsOrThreats;

public class EquipmentController {
    private ListInventory<InventoryItem> Inventory { get; }

    public EquipmentController(ListInventory<InventoryItem> inventory) => Inventory = inventory;

    public void EquipItem(Creature member, Equipment equip, int slot) {
        var equipSlot = Game.EquipmentOrder[slot];
        if (equip && equip.EquipSlot != equipSlot) return;

        if (member.Equips[slot]) Inventory.Add(member.Equips[slot], 1);
        if (equip) Inventory.Remove(equip, 1);

        member.Equips[slot] = equip;
    }
}