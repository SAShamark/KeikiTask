using UnityEngine;
using UnityEngine.UI;

namespace UI.Managers.Components
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridResizer : MonoBehaviour
    {
        [SerializeField]
        private bool _xCellSizeControl = true;

        [SerializeField]
        private bool _yCellSizeControl = true;

        [SerializeField]
        private RectTransform _contentRect;

        private GridLayoutGroup _gridLayout;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _rectTransform = _contentRect ? _contentRect : GetComponent<RectTransform>();
        }

        private void Start()
        {
            UpdateCellSize();
        }

        private void UpdateCellSize()
        {
            int columns = _gridLayout.constraintCount;
            float totalSpacing = _gridLayout.spacing.x * (columns - 1);
            float availableWidth = _rectTransform.rect.width - _gridLayout.padding.left - _gridLayout.padding.right -
                                   totalSpacing;
            float cellSize = availableWidth / columns;

            _gridLayout.cellSize = new Vector2(_xCellSizeControl ? cellSize : _gridLayout.cellSize.x,
                _yCellSizeControl ? cellSize : _gridLayout.cellSize.y);
        }
    }
}