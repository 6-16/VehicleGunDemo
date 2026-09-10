using UnityEngine;
using UnityEngine.EventSystems;

namespace ProBase
{
    public class PageSwipeHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private PagedView _pagedView;

        private RectTransform _rectTransform;
        private Vector2 _lastLocalPosition;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastLocalPosition = ToLocalPosition(eventData);
            _pagedView.BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPosition = ToLocalPosition(eventData);

            _pagedView.Drag(localPosition - _lastLocalPosition);
            _lastLocalPosition = localPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _pagedView.EndDrag();
        }

        private Vector2 ToLocalPosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPosition);

            return localPosition;
        }
    }
}
