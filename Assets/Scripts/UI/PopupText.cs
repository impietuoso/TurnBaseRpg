using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour {
    public TMP_Text text;

    public void Message(string message, Color color) {
        text.text = message;
        text.color = color;
    }

    public void AutoDestroy() {
        Destroy(gameObject);
    }
}