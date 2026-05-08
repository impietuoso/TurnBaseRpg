using UnityEngine;
using UnityEngine.EventSystems;

namespace Drafts.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class DraggableCollider2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Transform target;
        private Vector3 _offset;
        private Camera _mainCamera;

        private void Awake() => _mainCamera = Camera.main;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var worldPos = ScreenToWorld(eventData);
            _offset = target.transform.position - worldPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var worldPos = ScreenToWorld(eventData);
            target.transform.position = worldPos + _offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // optional: logic after release (e.g. snapping, constraints)
        }

        private Vector3 ScreenToWorld(PointerEventData eventData)
        {
            Vector3 screenPos = eventData.position;
            var z = Mathf.Abs(target.transform.position.z - _mainCamera.transform.position.z);
            return _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
        }
    }
}