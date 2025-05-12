using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Entities;
using Gameplay.Entities.Item;
using UnityEngine;

namespace UI.Screens.MainMenu.Data
{
    [Serializable]
    public class LevelData
    {
        [SerializeField]
        private string _groupName;

        [SerializeField]
        private string _soundName;

        [SerializeField]
        private ItemControl _itemControl;

        [SerializeField]
        private Sprite _icon;

        [SerializeField]
        private List<Color> _colors;

        public string GroupName => _groupName;
        public string SoundName => _soundName;

        public ItemControl ItemControl => _itemControl;
        public Sprite Sprite => _icon;
        public List<Color> Colors => _colors;
    }
}