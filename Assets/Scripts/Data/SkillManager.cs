using System;
using TricksAndTreatsOrThreats.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillManager : MonoBehaviour  {
    public CreatureView targetView;
    
    public void SwapSkills(GameObject drop, PointerEventData eventData) {
        throw new NotImplementedException();
        var target = drop.GetComponent<SkillView>().Data;
        var data = eventData.pointerDrag.GetComponent<SkillView>().Data;
        var dropParentList = (ObservableList<Skill>)drop.GetComponentInParent<ListView>().Data;
        var eventParentList = (ObservableList<Skill>)eventData.pointerDrag.GetComponentInParent<ListView>().Data;
        var targetIndex = drop.transform.GetSiblingIndex() - 1;
        var dataIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        dropParentList[targetIndex] = data;
        if(target == null && eventParentList != targetView.Data.Skills) eventParentList.RemoveAt(dataIndex);
        else eventParentList[dataIndex] = target;
    }

    public void UnequipSkill(SkillView view) {
        throw new NotImplementedException();
        if (view.Data == null) return;
        
        var targetIndex = view.transform.GetSiblingIndex() - 1;
        var removedSkill = targetView.Data.Skills[targetIndex];
        targetView.Data.Skills[targetIndex] = null;
    }
}