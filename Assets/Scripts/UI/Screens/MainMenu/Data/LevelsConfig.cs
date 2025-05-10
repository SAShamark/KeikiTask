using System.Collections.Generic;
using UnityEngine;

namespace UI.Screens.MainMenu
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Levels/Data")]
    public class LevelsConfig: ScriptableObject
    {
        [SerializeField]
        private List<LevelData> _levelData;

        public List<LevelData> LevelData => _levelData;
    }
}