using UnityEngine;
using UnityEngine.EventSystems;

public class PartyManager : MonoBehaviour {
    public LoadSave loadView;
    
    public void SwapPartyMember(GameObject drop, PointerEventData eventData) {
        var target = drop.GetComponent<PartyMemberView>().Data;
        var data = eventData.pointerDrag.GetComponent<PartyMemberView>().Data;
        var dropParentList = (ObservableList<PartyMember>)drop.GetComponentInParent<ListView>().Data;
        var eventParentList = (ObservableList<PartyMember>)eventData.pointerDrag.GetComponentInParent<ListView>().Data;
        var targetIndex = drop.transform.GetSiblingIndex() - 1;
        var dataIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        dropParentList[targetIndex] = data;
        eventParentList[dataIndex] = target;
    }

    public void RemoveFromParty(PartyMemberView view) {
        if (view.Data == null) return;
        
        var targetIndex = view.transform.GetSiblingIndex() - 1;
        var removedCharacter = loadView.save.currentParty[targetIndex];
        loadView.save.currentParty[targetIndex] = null;
        for (var i = 0; i < loadView.save.players.Count; i++) {
            if (loadView.save.players[i] == null) {
                loadView.save.players[i] = removedCharacter;
                break;
            }
        }
    }
}