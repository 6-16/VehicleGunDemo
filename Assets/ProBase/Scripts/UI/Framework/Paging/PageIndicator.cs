using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProBase
{
    public class PageIndicator : MonoBehaviour
    {
        [SerializeField] private PagedView _pagedView;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Image _dotPrefab;
        [SerializeField] private Color _activeColor = Color.white;
        [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.35f);

        private readonly List<Image> _dots = new List<Image>();

        private void OnEnable()
        {
            _pagedView.PagesChanged += Rebuild;
            _pagedView.PageChanged += Highlight;

            Rebuild();
        }

        private void OnDisable()
        {
            _pagedView.PagesChanged -= Rebuild;
            _pagedView.PageChanged -= Highlight;
        }

        private void Rebuild()
        {
            foreach (Image dot in _dots)
            {
                Destroy(dot.gameObject);
            }

            _dots.Clear();

            for (int index = 0; index < _pagedView.PageCount; index++)
            {
                _dots.Add(Instantiate(_dotPrefab, _container));
            }

            Highlight(_pagedView.CurrentPage);
        }

        private void Highlight(int page)
        {
            for (int index = 0; index < _dots.Count; index++)
            {
                _dots[index].color = index == page ? _activeColor : _inactiveColor;
            }
        }
    }
}
