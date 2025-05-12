using Audio;
using Gameplay.Entities;
using Gameplay.Entities.Item;
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
        private ItemController _item;
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

        private void SpawnItem()
        {
            _currentLevelData = _levelsManager.GetCurrentLevelData();
            
            ItemController item = Instantiate(_levelsManager.LevelsConfig.ItemController);
            Color color = _currentLevelData.Colors[_levelsManager.SelectedLevel];
            string soundName = _currentLevelData.SoundName;
            ItemControl itemControl = _currentLevelData.ItemControl;
            
            item.Initialize(_audioManager, _camera, color, soundName, itemControl);
            item.OnLevelCompleted += LevelCompleted;
            _item = item;
        }

        private void LevelCompleted()
        {
            _levelsManager.NextLevel();
            
            _currentLevelData = _levelsManager.GetCurrentLevelData();
            Color color = _currentLevelData.Colors[_levelsManager.SelectedLevel];
            string soundName = _currentLevelData.SoundName;
            
            _item.Init(color, soundName);
        }
    }
}