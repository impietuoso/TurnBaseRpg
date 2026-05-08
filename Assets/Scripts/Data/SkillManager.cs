using UnityEngine;
using UnityEngine.EventSystems;

public class SkillManager : MonoBehaviour  {
    public PartyMemberView memberView;
    
    public void SwapSkills(GameObject drop, PointerEventData eventData) {
        var target = drop.GetComponent<SkillView>().Data;
        var data = eventData.pointerDrag.GetComponent<SkillView>().Data;
        var dropParentList = (ObservableList<Skill>)drop.GetComponentInParent<ListView>().Data;
        var eventParentList = (ObservableList<Skill>)eventData.pointerDrag.GetComponentInParent<ListView>().Data;
        var targetIndex = drop.transform.GetSiblingIndex() - 1;
        var dataIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        dropParentList[targetIndex] = data;
        if(target == null && eventParentList != memberView.Data.equipedSkills) eventParentList.RemoveAt(dataIndex);
        else eventParentList[dataIndex] = target;
    }

    public void UnequipSkill(SkillView view) {
        if (view.Data == null) return;
        
        var targetIndex = view.transform.GetSiblingIndex() - 1;
        var removedSkill = memberView.Data.equipedSkills[targetIndex];
        memberView.Data.equipedSkills[targetIndex] = null;
        memberView.Data.learnedSkills.Add(removedSkill);
    }
}