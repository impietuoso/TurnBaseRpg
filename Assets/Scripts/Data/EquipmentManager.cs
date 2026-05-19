using System;
using System.Linq;
using TricksAndTreatsOrThreats.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour {
    public CreatureView targetView;
    public LoadSave load;
    public ListView inventoryView;
    public EquipmentView newItensStats;
    public EquipmentView currentItemStats;
    public Button swapButton;
    private int selectedItemIndex;
    private EquipSlot equipFilter = (EquipSlot)(-1);
    private EquipmentController _ctrl;
    
    private void OnEnable() {
        _ctrl ??= new (load.save.inventory); 
        ApplyFilter(EquipSlot.None);
    }

    public void ResetFilter() {
        var itensToFilter = load.save.inventory.slots.Where(i => i.item is Equipment);
        inventoryView.SetData(itensToFilter);
    }

    public void ApplyFilter(EquipmentView view) {
        equipFilter = Game.EquipmentOrder[view.transform.GetSiblingIndex() - 1];
        ApplyFilter(equipFilter);
    }

    public void ApplyFilter(EquipSlot newFilter) {
        targetView.SetData(targetView.Data);
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

        var removedEquipment = targetView.Data.Equips[selectedItemIndex];
        _ctrl.EquipItem(targetView.Data, newItensStats.Data, selectedItemIndex);

        newItensStats.SetData(removedEquipment);
        currentItemStats.SetData(targetView.Data.Equips[selectedItemIndex]);
        ApplyFilter(currentItemStats.Data.EquipSlot);
    }
    
    public void SwapFromEquipment(GameObject drop, PointerEventData eventData) {
        if (eventData.pointerDrag.transform.parent == drop.transform.parent) return;

        var inventoryItem = drop.GetComponent<EquipmentView>().Data;
        var index = eventData.pointerDrag.transform.GetSiblingIndex() - 1;

        _ctrl.EquipItem(targetView.Data, inventoryItem, index);
        ApplyFilter(equipFilter);
    }

    public void SwapFromInventory(GameObject drop, PointerEventData eventData) {
        if (eventData.pointerDrag.transform.parent == drop.transform.parent) return;

        var inventoryItem = eventData.pointerDrag.GetComponent<EquipmentView>().Data;
        var drawIndex = drop.transform.GetSiblingIndex() - 1;

        _ctrl.EquipItem(targetView.Data, inventoryItem, drawIndex);
        ApplyFilter(equipFilter);
    }

    public void UnequipEquipment(EquipmentView view) {
        if (!view.Data) return;

        var drawIndex = view.transform.GetSiblingIndex() - 1;

        _ctrl.EquipItem(targetView.Data, null, drawIndex);
        ApplyFilter(equipFilter);
    }
}