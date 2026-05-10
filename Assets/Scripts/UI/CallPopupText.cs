using System.Collections;
using Drafts;
using UnityEngine;

public class CallPopupText : MonoBehaviour {
    [Prefab] public PopupText popup;
    
    public void CreatePopup(string message, Color color, Transform target) {
        StartCoroutine(Pop(message, color, target, 0));
    }
    
    public void CreatePopup(string message, Color color, Transform target, float delay) {
        StartCoroutine(Pop(message, color, target, delay));
    }

    public IEnumerator Pop(string message, Color color, Transform target, float delay) {
        yield return new WaitForSeconds(delay);
        var newPopup = Instantiate(popup, target);
        newPopup.GetComponent<PopupText>().Message(message, color);
        newPopup.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1.75f);
    }
}