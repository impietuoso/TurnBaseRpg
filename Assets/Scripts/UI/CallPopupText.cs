using System.Collections;
using Drafts;
using UnityEngine;

public class CallPopupText : MonoBehaviour {
    [Prefab] public PopupText popup;
    
    public IEnumerator Pop(string message, Color color, Transform target, float delay = 0) {
        yield return new WaitForSeconds(delay);
        var newPopup = Instantiate(popup, target.transform);
        newPopup.GetComponent<PopupText>().Message(message, color);
        newPopup.gameObject.SetActive(true);
    }
}