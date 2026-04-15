using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour  {
    public PartyMemberView memberView;
    public LoadSave load;
    public ListView inventoryView;
    public EquipmentView newItensStats;
    public EquipmentView currentItemStats;
    public Button swapButton;
    private int selectedItemIndex;
    private EquipmentType equipFilter = null;

    private void OnEnable() {
        ApplyFilter((EquipmentType) null);
    }

    public void ResetFilter() {
        var itensToFilter = load.save.inventory.slots.Where(i => i.item is Equipment);
        inventoryView.SetData(itensToFilter);
    }

    public void ApplyFilter(EquipmentView view) {
        int orderIndex = GameConfig.Instance.equipmentDrawOrder[view.transform.GetSiblingIndex() - 1];
        equipFilter = GameConfig.Instance.equipmentArrayOrder[orderIndex];
        ApplyFilter(equipFilter);
    }
    
    public void ApplyFilter(EquipmentType newFilter) {
        memberView.SetData(memberView.data);
        equipFilter = newFilter;
        if (newFilter == null) {
            ResetFilter();
            return;
        }
        if(newItensStats.data != null && newItensStats.data.equipmentType != newFilter) newItensStats.SetData(null); 
        var itensToFilter = load.save.inventory.slots.Where(
            i => i.item is Equipment e && e.equipmentType == newFilter);
        inventoryView.SetData(itensToFilter);
        swapButton.interactable = false;
    }
    

    public void SelectNewEquipButton() {
        List<int> weaponIndexes = new();
        for (int i = 0; i < memberView.data.equips.Count; i++) {
            var type = GameConfig.Instance.equipmentArrayOrder[i];
            if (type == newItensStats.data.equipmentType) {
                weaponIndexes.Add(i);
            }
        }

        var firstSlot = memberView.data.equips[weaponIndexes[0]];
        var secondSlot = memberView.data.equips[weaponIndexes[1]];
        
        if (firstSlot == null || firstSlot is Weapon w && w.twoHanded) {
            currentItemStats.SetData(firstSlot);
            selectedItemIndex = weaponIndexes[0];
        } else if(secondSlot == null) {
            currentItemStats.SetData(secondSlot);
            selectedItemIndex = weaponIndexes[1];
        } else {
            currentItemStats.SetData(firstSlot);
            selectedItemIndex = weaponIndexes[0];
        }
        
        swapButton.interactable = true;
    }

    public void SwapEquipmentButton() {
        if (newItensStats.data == null) return;

        var removedEquipment = memberView.data.equips[selectedItemIndex];
        
        EquipmentController.EquipItem(load, memberView.data, newItensStats.data, selectedItemIndex);
        
        newItensStats.SetData(removedEquipment);
        currentItemStats.SetData(memberView.data.equips[selectedItemIndex]);
        ApplyFilter(currentItemStats.data.equipmentType);
    }
    

    public void SwapFromEquipment(GameObject drop, PointerEventData eventData) {
        if (eventData.pointerDrag.transform.parent == drop.transform.parent) return;
        
        var inventoryItem = drop.GetComponent<EquipmentView>().data;
        var drawIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        var equipedItemIndex = GameConfig.Instance.equipmentDrawOrder[drawIndex];
        
        EquipmentController.EquipItem(load, memberView.data, inventoryItem, equipedItemIndex);
        
        ApplyFilter(equipFilter);
    }

    public void SwapFromInventory(GameObject drop, PointerEventData eventData) {
        if (eventData.pointerDrag.transform.parent == drop.transform.parent) return;

        var inventoryItem = eventData.pointerDrag.GetComponent<EquipmentView>().data;
        var drawIndex = drop.transform.GetSiblingIndex() - 1;
        var equipedItemIndex = GameConfig.Instance.equipmentDrawOrder[drawIndex];
        
        EquipmentController.EquipItem(load, memberView.data, inventoryItem, equipedItemIndex);
        
        ApplyFilter(equipFilter);
    }

    public void UnequipEquipment(EquipmentView view) {
        if (view.data == null) return;
        
        var drawIndex = view.transform.GetSiblingIndex() - 1;
        var targetIndex = GameConfig.Instance.equipmentDrawOrder[drawIndex];
        
        EquipmentController.EquipItem(load, memberView.data, null, targetIndex);
        
        ApplyFilter(equipFilter);
    }
}