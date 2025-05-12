using System.Collections.Generic;
using Gameplay;
using Gameplay.Entities;
using Gameplay.Entities.Item;
using UnityEngine;

namespace UI.Screens.MainMenu.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Levels/Data")]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField]
        private List<LevelData> _levelData;

        [SerializeField]
        private ItemController _itemController;

        public List<LevelData> LevelData => _levelData;
        public ItemController ItemController => _itemController;
    }
}