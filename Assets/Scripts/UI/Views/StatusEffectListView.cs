using System.Collections.Generic;
using UnityEngine;

public class StatusEffectListView : MonoBehaviour {
    public StatusEffectView template;
    public StatusEffectList owner;
    private Dictionary<Status, StatusEffectView> instantiatedViews = new Dictionary<Status, StatusEffectView>();

    private void Awake() {
        template.gameObject.SetActive(false);
    }

    public void SetData(StatusEffectList effectList) {
        ClearData();
        
        owner = effectList;
        owner.OnStatusAdded += OnStatusAdded;
        owner.OnStatusRemoved += OnStatusRemoved;

        foreach (var kvp in owner.StatusList) {
            OnStatusAdded(kvp.Value);
        }
    }

    public void ClearData() {
        if (owner == null) return;
        
        foreach (var par in instantiatedViews) {
            Destroy(par.Value.gameObject);
        }
        instantiatedViews.Clear();
        owner.OnStatusAdded -= OnStatusAdded;
        owner.OnStatusRemoved -= OnStatusRemoved;
        owner = null;
    }

    private void OnStatusAdded(Status status) {
        StatusEffectView newView = Instantiate(template, template.transform.parent);
        newView.gameObject.SetActive(true);
        newView.SetInfo(status);
        instantiatedViews.Add(status, newView);
    }

    private void OnStatusRemoved(Status status) {
        if (instantiatedViews.TryGetValue(status, out StatusEffectView view)) {
            Destroy(view.gameObject);
            instantiatedViews.Remove(status);
        }
    }

    private void OnDestroy() {
        if (owner != null) {
            owner.OnStatusAdded -= OnStatusAdded;
            owner.OnStatusRemoved -= OnStatusRemoved;
        }
    }
}
