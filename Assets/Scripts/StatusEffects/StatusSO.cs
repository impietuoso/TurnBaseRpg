using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Status", fileName = "New Status")]
public class StatusSO : ScriptableObject {
    public Sprite statusIcon;
    [SerializeReference, TypeDropdown]
    public Status status;
    public StatusType statusType;
    public Color statusPopupColor;

    public Status Clone() {
        var clone = UnityEngine.Object.Instantiate(this).status;
        clone.source = this;
        return clone;
    }
}