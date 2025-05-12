using UnityEngine;

namespace Gameplay.Entities.Item
{
    public class Eraser : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _targetRenderer;

        private readonly float _eraseRadius = 0.8f;

        private Transform _eraserTransform;

        private Texture2D _texture;
        private Sprite _originalSprite;

        private Texture2D _originalTexture;
        private bool _isPaused = true;

        private void Start()
        {
            InitializeTexture();
        }

        private void Update()
        {
            if (_eraserTransform != null && !_isPaused)
            {
                EraseAtPosition(_eraserTransform.position);
            }
        }

        public void Initialize(Transform follower)
        {
            _eraserTransform = follower;
        }

        private void InitializeTexture()
        {
            _originalSprite = _targetRenderer.sprite;

            CopySpriteTexture();
            SaveCopyTexture();
            CreateNewSprite();
        }

        private void CopySpriteTexture()
        {
            Texture2D sourceTex = _originalSprite.texture;
            _texture = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.RGBA32, false);
            _texture.SetPixels(sourceTex.GetPixels());
            _texture.Apply();
        }

        private void SaveCopyTexture()
        {
            _originalTexture = new Texture2D(_texture.width, _texture.height);
            _originalTexture.SetPixels(_texture.GetPixels());
            _originalTexture.Apply();
        }

        private void CreateNewSprite()
        {
            _targetRenderer.sprite = Sprite.Create(
                _texture,
                _originalSprite.rect,
                new Vector2(0.5f, 0.5f),
                _originalSprite.pixelsPerUnit
            );
        }

        private void EraseAtPosition(Vector2 worldPos)
        {
            Vector2 localPos = worldPos - (Vector2)_targetRenderer.transform.position;

            float pixelsPerUnit = _texture.width / _originalSprite.bounds.size.x;
            var px = (int)((localPos.x + _originalSprite.bounds.size.x / 2f) * pixelsPerUnit);
            var py = (int)((localPos.y + _originalSprite.bounds.size.y / 2f) * pixelsPerUnit);

            int radius = Mathf.RoundToInt(_eraseRadius * pixelsPerUnit);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int tx = px + x;
                    int ty = py + y;

                    if (tx >= 0 && tx < _texture.width && ty >= 0 && ty < _texture.height)
                    {
                        float dist = Mathf.Sqrt(x * x + y * y);
                        if (dist <= radius)
                        {
                            Color pixel = _texture.GetPixel(tx, ty);
                            pixel.a = 0f;
                            _texture.SetPixel(tx, ty, pixel);
                        }
                    }
                }
            }

            _texture.Apply();
        }

        public void SwitchPauseErased(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void ClearErased()
        {
            _texture.SetPixels(_originalTexture.GetPixels());
            _texture.Apply();
        }
    }
}