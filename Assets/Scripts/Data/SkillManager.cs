using UnityEngine;
using UnityEngine.EventSystems;

public class SkillManager : MonoBehaviour  {
    public PartyMemberView memberView;
    
    public void SwapSkills(GameObject drop, PointerEventData eventData) {
        var target = drop.GetComponent<SkillView>().data;
        var data = eventData.pointerDrag.GetComponent<SkillView>().data;
        var dropParentList = (ObservableList<Skill>)drop.GetComponentInParent<ListView>().data;
        var eventParentList = (ObservableList<Skill>)eventData.pointerDrag.GetComponentInParent<ListView>().data;
        var targetIndex = drop.transform.GetSiblingIndex() - 1;
        var dataIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        dropParentList[targetIndex] = data;
        if(target == null && eventParentList != memberView.data.equipedSkills) eventParentList.RemoveAt(dataIndex);
        else eventParentList[dataIndex] = target;
    }

    public void UnequipSkill(SkillView view) {
        if (view.data == null) return;
        
        var targetIndex = view.transform.GetSiblingIndex() - 1;
        var removedSkill = memberView.data.equipedSkills[targetIndex];
        memberView.data.equipedSkills[targetIndex] = null;
        memberView.data.learnedSkills.Add(removedSkill);
    }
}