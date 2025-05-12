using System;
using Audio;
using Services.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Entities.Item
{
    [Serializable]
    public class TipsManager
    {
        [SerializeField]
        private Transform _handTip;

        [SerializeField]
        private GameObject _circlePrefab;

        [SerializeField]
        private int _startCircleCount = 4;

        [SerializeField]
        private GameObject _starPrefab;

        [SerializeField]
        private Transform _tipsContainer;

        public GameObject Star { get; private set; }
        public ObjectPool CircleObjectPool { get; private set; }
        
        public Transform HandTip => _handTip;

        public void Initialize(IAudioManager audioManager, string soundName)
        {
            CircleObjectPool = new ObjectPool(_circlePrefab, _startCircleCount, _tipsContainer);
            Star = Object.Instantiate(_starPrefab, _tipsContainer);
            Star.SetActive(false);
        }


        internal void HandlePointVisualization(bool isLastPoint, Vector3 point)
        {
            if (isLastPoint)
            {
                Star.transform.position = point;
                Star.SetActive(true);
            }
            else
            {
                GameObject circle = CircleObjectPool.GetFreeElement();
                circle.transform.position = point;
            }
        }
    }
}