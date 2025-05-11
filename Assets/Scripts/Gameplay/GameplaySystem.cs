using Audio;
using UI.Screens.MainMenu.Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameplaySystem : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private LevelsManager _levelsManager;
        private ItemControl _item;
        private LevelData _currentLevelData;
        private IAudioManager _audioManager;

        [Inject]
        private void Construct(LevelsManager levelsManager, IAudioManager audioManager)
        {
            _levelsManager = levelsManager;
            _audioManager = audioManager;
        }

        private void Start()
        {
            SpawnItem();
        }

        private void OnDestroy()
        {
            if (_item != null)
            {
                _item.OnLevelCompleted -= LevelCompleted;
            }
        }

        private void LevelCompleted()
        {
            Destroy(_item.gameObject);
            _levelsManager.NextLevel();
            SpawnItem();
        }

        private void SpawnItem()
        {
            _currentLevelData = _levelsManager.GetCurrentLevelData();
            ItemControl item = Instantiate(_currentLevelData.ItemControl);
            item.Initialize(_audioManager, _camera, _currentLevelData.Colors[_levelsManager.SelectedLevel],
                _currentLevelData.SoundName);
            item.OnLevelCompleted += LevelCompleted;
            _item = item;
        }
    }
}