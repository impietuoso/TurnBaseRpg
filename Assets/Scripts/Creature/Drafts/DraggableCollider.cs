using UnityEngine;
using UnityEngine.EventSystems;

namespace Drafts.UI
{
    public class DraggableCollider : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Tooltip("Optional: Plane normal to constrain dragging (e.g., Vector3.up for XZ plane). Leave zero for free movement.")]
        public Vector3 dragPlaneNormal = Vector3.zero;

        private Vector3 _offset;
        private Plane _dragPlane;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.worldPosition == Vector3.zero) return;
            _offset = transform.position - eventData.pointerCurrentRaycast.worldPosition;

            var planeNormal = dragPlaneNormal == Vector3.zero ? eventData.pointerCurrentRaycast.worldNormal : dragPlaneNormal;
            _dragPlane = new Plane(planeNormal, transform.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

            if (!_dragPlane.Raycast(ray, out var distance)) return;
            var worldPos = ray.GetPoint(distance);
            transform.position = worldPos + _offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // optional: snap or apply physics here
        }
    }
}