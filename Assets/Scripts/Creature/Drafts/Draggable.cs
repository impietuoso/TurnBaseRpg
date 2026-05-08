using UnityEngine;
using UnityEngine.EventSystems;

namespace Drafts.UI
{
    public class SimpleDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public CanvasGroup target;
        public float dragAlpha = 1;

        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _rectTransform = target.GetComponent<RectTransform>();
            _canvas = target.GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!target) return;
            target.alpha = dragAlpha;
            target.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!target) return;
            target.alpha = 1f;
            target.blocksRaycasts = true;
        }
    }
}