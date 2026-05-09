using UnityEngine;
using UnityEngine.EventSystems;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class PopupHandler : MonoBehaviour, IPointerClickHandler
    {
        private static Canvas _parent;

        [SerializeField] private DataView popup;
        [SerializeField] private DataView dataView;

        private DataView _current;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_current)
            {
                _current.gameObject.SetActive(true);
                return;
            }
            
            if (!_parent) _parent = FindFirstObjectByType<Canvas>();

            if (eventData.button != PointerEventData.InputButton.Right) return;
            eventData.Use();

            var clone = Instantiate(popup, eventData.position, Quaternion.identity, _parent.transform);
            clone.SetData(dataView.GetData());
        }
    }
}