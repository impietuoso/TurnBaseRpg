using UnityEngine;
using UnityEngine.EventSystems;

public class PartyManager : MonoBehaviour {
    public LoadSave loadView;
    
    public void SwapPartyMember(GameObject drop, PointerEventData eventData) {
        var target = drop.GetComponent<PartyMemberView>().data;
        var data = eventData.pointerDrag.GetComponent<PartyMemberView>().data;
        var dropParentList = (ObservableList<PartyMember>)drop.GetComponentInParent<ListView>().data;
        var eventParentList = (ObservableList<PartyMember>)eventData.pointerDrag.GetComponentInParent<ListView>().data;
        var targetIndex = drop.transform.GetSiblingIndex() - 1;
        var dataIndex = eventData.pointerDrag.transform.GetSiblingIndex() - 1;
        dropParentList[targetIndex] = data;
        eventParentList[dataIndex] = target;
    }

    public void RemoveFromParty(PartyMemberView view) {
        if (view.data == null) return;
        
        var targetIndex = view.transform.GetSiblingIndex() - 1;
        var removedCharacter = loadView.save.currentParty[targetIndex];
        loadView.save.currentParty[targetIndex] = null;
        for (int i = 0; i < loadView.save.players.Count; i++) {
            if (loadView.save.players[i] == null) {
                loadView.save.players[i] = removedCharacter;
                break;
            }
        }
    }
}