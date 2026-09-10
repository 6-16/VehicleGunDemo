using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProBase
{
    public class PagedView : MonoBehaviour
    {
        [SerializeField] private RectTransform _viewport;
        [SerializeField] private RectTransform _content;
        [SerializeField] private PageAxis _axis = PageAxis.Horizontal;
        [SerializeField] private float _snapSpeed = 12f;

        [Range(0.05f, 0.9f)]
        [SerializeField] private float _swipeThreshold = 0.25f;

        private readonly List<RectTransform> _pages = new List<RectTransform>();

        private int _currentPage;
        private bool _isDragging;

        public event Action<int> PageChanged;
        public event Action PagesChanged;

        public int PageCount => _pages.Count;
        public int CurrentPage => _currentPage;
        public PageAxis Axis => _axis;

        private Vector2 PageOffset => _axis == PageAxis.Horizontal
            ? new Vector2(_viewport.rect.width, 0f)
            : new Vector2(0f, -_viewport.rect.height);

        private float PageLength => Mathf.Max(1f, PageOffset.magnitude);

        private Vector2 TargetPosition => -PageOffset * _currentPage;

        public void SetPages(IReadOnlyList<RectTransform> pages)
        {
            _pages.Clear();

            for (int index = 0; index < pages.Count; index++)
            {
                RectTransform page = pages[index];

                page.SetParent(_content, false);
                page.anchoredPosition = PageOffset * index;

                _pages.Add(page);
            }

            _currentPage = Mathf.Clamp(_currentPage, 0, Mathf.Max(0, _pages.Count - 1));
            _content.anchoredPosition = TargetPosition;

            PagesChanged?.Invoke();
            PageChanged?.Invoke(_currentPage);
        }

        public void GoTo(int index, bool animate = true)
        {
            if (_pages.Count == 0) return;

            int clamped = Mathf.Clamp(index, 0, _pages.Count - 1);
            bool changed = clamped != _currentPage;

            _currentPage = clamped;

            if (!animate)
            {
                _content.anchoredPosition = TargetPosition;
            }

            if (changed)
            {
                PageChanged?.Invoke(_currentPage);
            }
        }

        public void Next()
        {
            GoTo(_currentPage + 1);
        }

        public void Previous()
        {
            GoTo(_currentPage - 1);
        }

        public void BeginDrag()
        {
            _isDragging = true;
        }

        public void Drag(Vector2 delta)
        {
            if (!_isDragging || _pages.Count == 0) return;

            Vector2 direction = PageOffset.normalized;
            Vector2 moved = _content.anchoredPosition + direction * Vector2.Dot(delta, direction);

            _content.anchoredPosition = ClampToPages(moved);
        }

        public void EndDrag()
        {
            _isDragging = false;

            if (_pages.Count == 0) return;

            float travelled = -Project(_content.anchoredPosition - TargetPosition) / PageLength;

            if (travelled >= _swipeThreshold)
            {
                Next();
            }
            else if (travelled <= -_swipeThreshold)
            {
                Previous();
            }
        }

        private void Update()
        {
            if (_isDragging || _pages.Count == 0) return;

            _content.anchoredPosition = Vector2.Lerp(
                _content.anchoredPosition,
                TargetPosition,
                1f - Mathf.Exp(-_snapSpeed * Time.unscaledDeltaTime));
        }

        private float Project(Vector2 value)
        {
            return Vector2.Dot(value, PageOffset.normalized);
        }

        private Vector2 ClampToPages(Vector2 position)
        {
            Vector2 direction = PageOffset.normalized;
            float projected = Vector2.Dot(position, direction);
            float limit = PageLength * (_pages.Count - 1);

            return direction * Mathf.Clamp(projected, -limit, 0f);
        }
    }
}
