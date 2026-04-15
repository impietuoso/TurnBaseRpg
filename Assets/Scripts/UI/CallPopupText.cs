using System.Collections;
using UnityEngine;

public class CallPopupText : MonoBehaviour {
    public PopupText popup;
    
    public void CreatePopup(string message, Color color, Transform target) {
        StartCoroutine(Pop(message, color, target, 0));
    }
    
    public void CreatePopup(string message, Color color, Transform target, float delay) {
        StartCoroutine(Pop(message, color, target, delay));
    }

    public IEnumerator Pop(string message, Color color, Transform target, float delay) {
        yield return new WaitForSeconds(delay);
        var newPopup = Instantiate(popup, target.position, Quaternion.identity, target);
        newPopup.GetComponent<PopupText>().Message(message, color);
        newPopup.gameObject.SetActive(true);
    }
}