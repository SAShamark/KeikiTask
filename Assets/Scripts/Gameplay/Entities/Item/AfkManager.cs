using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Audio.Data;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Entities.Item
{
    [Serializable]
    public class AfkManager
    {
        [SerializeField]
        private int _secondsToTips = 7;

        [SerializeField]
        private float _handDurationPerSegment = 0.5f;

        private Transform _handTip;
        private IAudioManager _audioManager;
        private string _soundName;

        public event Action OnMoveHand;

        public void Initialize(IAudioManager audioManager, string soundName, Transform handTip)
        {
            _audioManager = audioManager;
            _soundName = soundName;
            _handTip = handTip;
        }

        internal IEnumerator Manage()
        {
            yield return new WaitForSeconds(_secondsToTips);
            _audioManager.Play(AudioGroupType.EffectSounds, _soundName);
            yield return new WaitForSeconds(_secondsToTips);
            OnMoveHand?.Invoke();
        }


        public void MoveHandTip(List<Vector3> routePoints, int currentPointIndex)
        {
            if (routePoints.Count == 0)
            {
                return;
            }

            _handTip.position = routePoints[currentPointIndex];
            _handTip.gameObject.SetActive(true);

            for (int i = currentPointIndex; i < routePoints.Count; i++)
            {
                Vector3 target = routePoints[i];
                Tween moveTween = _handTip.DOMove(target, _handDurationPerSegment).SetEase(Ease.Linear);
                moveTween.WaitForCompletion();
            }

            _handTip.gameObject.SetActive(false);
        }
    }
}