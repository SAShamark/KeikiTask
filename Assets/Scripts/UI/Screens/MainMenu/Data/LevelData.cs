using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Screens.MainMenu
{
    [Serializable]
    public class LevelData
    {
        [SerializeField]
        private string _groupName;

        [SerializeField]
        private Sprite _icon;

        [SerializeField]
        private List<Color> _colors;

        public string GroupName => _groupName;

        public Sprite Sprite => _icon;
        public List<Color> Colors => _colors;
    }
}