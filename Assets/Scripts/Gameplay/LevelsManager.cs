using System;
using Services;
using UI.Screens.MainMenu.Data;

namespace Gameplay
{
    public class LevelsManager : ILoadingInitialization
    {
        public LevelsConfig LevelsConfig { get; private set; }
        public int SelectedGroupIndex { get; private set; }
        public int SelectedLevel { get; private set; }
        public int Priority => 1;

        private LevelsManager(LevelsConfig levelsConfig)
        {
            LevelsConfig = levelsConfig;
        }

        public void Init()
        {
        }

        public void SelectLevel(int groupIndex, int level)
        {
            SelectedGroupIndex = groupIndex;
            SelectedLevel = level;
        }

        public void NextLevel()
        {
            SelectedLevel += 1;

            var colors = GetCurrentLevelData().Colors;
            if (SelectedLevel >= colors.Count)
            {
                SelectedLevel = 0;
            }
        }

        public LevelData GetCurrentLevelData()
        {
            return LevelsConfig.LevelData[SelectedGroupIndex];
        }
    }
}