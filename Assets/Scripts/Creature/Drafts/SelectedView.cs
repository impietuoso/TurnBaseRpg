using UnityEngine;
using UnityEngine.Events;

namespace Drafts
{
    public class SelectedView : MonoBehaviour
    {
        [SerializeField] private DataView referenceValue;
        [SerializeField] private DataView selfValue;

        public UnityEvent<bool> onChanged;
        public UnityEvent onSelected;
        public UnityEvent onNotSelected;

        public bool IsSelected { get; private set; }

        private void Awake()
        {
            //TODO
            // referenceValue.onDataChanged.AddListener(Refresh);
            // selfValue.onDataChanged.AddListener(Refresh);
        }

        private void OnEnable() => Refresh(null);

        private void Refresh(object _)
        {
            IsSelected = selfValue.GetData() is not null &&
                         selfValue.GetData().Equals(referenceValue.GetData());

            onChanged.Invoke(IsSelected);
            if (IsSelected) onSelected.Invoke();
            else onNotSelected.Invoke();
        }
    }
}