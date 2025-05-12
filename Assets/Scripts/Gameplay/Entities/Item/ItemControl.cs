using UnityEngine;

namespace Gameplay.Entities.Item
{
    public class ItemControl : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer[] _routePaths;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public LineRenderer[] RoutePaths => _routePaths;

        public void Initialize(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}