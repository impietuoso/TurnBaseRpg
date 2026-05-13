using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour {
    public PartyMemberView memberView;
    public LoadSave load;
    public ListView inventoryView;
    public EquipmentView newItensStats;
    public EquipmentView currentItemStats;
    public Button swapButton;
    private int selectedItemIndex;
    private EquipSlot equipFilter = (EquipSlot)(-1);

    private void OnEnable() {
        ApplyFilter(EquipSlot.None);
    }

    public void ResetFilter() {
        var itensToFilter = load.save.inventory.slots.Where(i => i.item is Equipment);
        inventoryView.SetData(itensToFilter);
    }

    public void ApplyFilter(EquipmentView view) {
        var orderIndex = Game.Config.equipmentDrawOrder[view.transform.GetSiblingIndex() - 1];
        equipFilter = Game.Config.equipmentOrder[orderIndex];
        ApplyFilter(equipFilter);
    }

    public void ApplyFilter(EquipSlot newFilter) {
        memberView.SetData(memberView.Data);
        equipFilter = newFilter;
        if (newFilter == EquipSlot.None) {
            ResetFilter();
            return;
        }
        if (newItensStats.Data != null && newItensStats.Data.EquipSlot != newFilter) newItensStats.SetData(null);
        var itensToFilter = load.save.inventory.slots.Where(
            i => i.item is Equipment e && e.EquipSlot == newFilter);
        inventoryView.SetData(itensToFilter);
        swapButton.interactable = false;
    }

    public void SelectNewEquipButton() {
        throw new NotImplementedException();
        // currentItemStats.SetData(firstSlot);
        // selectedItemIndex = weaponIndexes[0];
        swapButton.interactable = true;
    }

    public void SwapEquipmentButton() {
        if (newItensStats.Data == null) return;

        var removedEquipment = memberView.Data.equips[selectedItemIndex];

        EquipmentController.EquipItem(load, memberView.Data, newItensStats.Data, selectedItemIndex);

        newItensStats.SetData(removedEquipment);
        currentItemStats.SetData(memberView.Data.equips[selectedItemIndex]);
        ApplyFilter(currentItemStats.Data.EquipSlot);
    }


    public void SwapFromEquipment(GameObject drop, PointerEventData eventData) {
        if (eventData.pointerDrag.transform.parent == drop.transform.parent) return;

        var inventoryItem = drop.GetComponent<EquipmentView>().Data;
        var drawIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        var equipedItemIndex = Game.Config.equipmentDrawOrder[drawIndex];

        EquipmentController.EquipItem(load, memberView.Data, inventoryItem, equipedItemIndex);

        ApplyFilter(equipFilter);
    }

    public void SwapFromInventory(GameObject drop, PointerEventData eventData) {
        if (eventData.pointerDrag.transform.parent == drop.transform.parent) return;

        var inventoryItem = eventData.pointerDrag.GetComponent<EquipmentView>().Data;
        var drawIndex = drop.transform.GetSiblingIndex() - 1;
        var equipedItemIndex = Game.Config.equipmentDrawOrder[drawIndex];

        EquipmentController.EquipItem(load, memberView.Data, inventoryItem, equipedItemIndex);

        ApplyFilter(equipFilter);
    }

    public void UnequipEquipment(EquipmentView view) {
        if (view.Data == null) return;

        var drawIndex = view.transform.GetSiblingIndex() - 1;
        var targetIndex = Game.Config.equipmentDrawOrder[drawIndex];

        EquipmentController.EquipItem(load, memberView.Data, null, targetIndex);

        ApplyFilter(equipFilter);
    }
}