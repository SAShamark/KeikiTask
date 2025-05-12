using UnityEngine;

namespace Gameplay.Entities.Item
{
    public class ItemControl : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer[] _routePaths;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Eraser _eraser;

        public LineRenderer[] RoutePaths => _routePaths;

        public void Initialize(Color color, Transform follower)
        {
            _spriteRenderer.color = color;
            _eraser.Initialize(follower);
        }

        public void ClearErased() => _eraser.ClearErased();
        public void SwitchPauseErased(bool isPaused) => _eraser.SwitchPauseErased(isPaused);
    }
}