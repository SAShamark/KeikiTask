using System;
using System.Collections;
using Audio;
using Audio.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Entities.Item
{
    public class ItemController : MonoBehaviour
    {
        [SerializeField]
        private float _threshold = 0.5f;

        [SerializeField]
        private float _followSpeed = 10f;

        [SerializeField]
        private Transform _follower;

        [SerializeField]
        private Transform _levelItemContainer;

        [SerializeField]
        private TipsManager _tipsManager;

        [SerializeField]
        private RouteManager _routeManager;

        [SerializeField]
        private AfkManager _afkManager;

        private Camera _camera;
        private IAudioManager _audioManager;

        private int _currentRouteIndex;
        private int _currentPointIndex;
        private bool _isTracing;

        private Coroutine _audioCoroutine;
        private Coroutine _afkCoroutine;

        private ItemControl _levelItem;

        public event Action OnLevelCompleted;

        private void Update()
        {
            HandleInput();
            TraceRoute();
        }

        private void OnDestroy()
        {
            if (_audioCoroutine != null)
            {
                StopCoroutine(_audioCoroutine);
            }

            if (_afkCoroutine != null)
            {
                StopCoroutine(_afkCoroutine);
            }
        }

        public void Initialize(IAudioManager audioManager, Camera camera, Color color, string soundName,
            ItemControl itemControl)
        {
            _camera = camera;
            _audioManager = audioManager;

            _tipsManager.Initialize(_audioManager, soundName);

            _routeManager.Initialize(_tipsManager);

            _afkManager.Initialize(audioManager, soundName, _tipsManager.HandTip);
            _afkManager.OnMoveHand += MoveHand;

            _levelItem = Instantiate(itemControl, _levelItemContainer);

            Init(color, soundName);
        }

        public void Init(Color color, string soundName)
        {
            ResetData();

            _levelItem.Initialize(color);


            _audioCoroutine = StartCoroutine(PlayAudioThenLoad(soundName, () => NextStep(_currentRouteIndex)));
            _afkCoroutine = StartCoroutine(_afkManager.Manage());

            if (_routeManager.RoutePoints.Count > 0)
            {
                _follower.position = _routeManager.RoutePoints[0];
            }
        }

        private void ResetData()
        {
            _currentRouteIndex = 0;

            if (_audioCoroutine != null)
            {
                StopCoroutine(_audioCoroutine);
            }

            if (_afkCoroutine != null)
            {
                StopCoroutine(_afkCoroutine);
            }
        }

        private void MoveHand() => _afkManager.MoveHandTip(_routeManager.RoutePoints, _currentPointIndex);

        private IEnumerator PlayAudioThenLoad(string audioName, Action action)
        {
            AudioSource source = _audioManager.Play(AudioGroupType.EffectSounds, audioName);
            yield return new WaitWhile(() => source.isPlaying);
            action?.Invoke();
        }


        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_afkCoroutine != null)
                {
                    StopCoroutine(_afkCoroutine);
                }

                _tipsManager.HandTip.gameObject.SetActive(false);

                _isTracing = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _afkCoroutine = StartCoroutine(_afkManager.Manage());
                _isTracing = false;
            }
        }

        private void TraceRoute()
        {
            if (_isTracing && _currentPointIndex < _routeManager.RoutePoints.Count)
            {
                Vector3 touchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0f;

                float distance = Vector3.Distance(touchPosition, _routeManager.RoutePoints[_currentPointIndex]);
                if (distance <= _threshold)
                {
                    _currentPointIndex++;

                    if (_currentPointIndex >= _routeManager.RoutePoints.Count)
                    {
                        _currentRouteIndex++;
                        if (_currentRouteIndex < _levelItem.RoutePaths.Length)
                        {
                            NextStep(_currentRouteIndex);
                        }
                        else
                        {
                            CompleteLevel();
                        }
                    }
                }

                if (_currentPointIndex < _routeManager.RoutePoints.Count)
                {
                    _follower.position = Vector3.MoveTowards(_follower.position,
                        _routeManager.RoutePoints[_currentPointIndex],
                        _followSpeed * Time.deltaTime);
                }
            }
        }

        private void NextStep(int index)
        {
            if (index >= _levelItem.RoutePaths.Length)
            {
                return;
            }

            _tipsManager.CircleObjectPool.TurnOffAllPool();

            _routeManager.ProcessRoute(_levelItem.RoutePaths[index]);
            _currentPointIndex = 0;
            FollowerPreparing();
        }

        private void FollowerPreparing()
        {
            if (_routeManager.RoutePoints.Count > 0)
            {
                _follower.position = _routeManager.RoutePoints[0];
            }

            _follower.gameObject.SetActive(true);
        }

        private void CompleteLevel()
        {
            Deactivate();

            string[] soundNames =
                { SoundsConstants.AWESOME, SoundsConstants.EXCELLENT, SoundsConstants.THATS_GOOD };
            string randomSound = soundNames[Random.Range(0, soundNames.Length)];
            StartCoroutine(PlayAudioThenLoad(randomSound, () => OnLevelCompleted?.Invoke()));
        }

        private void Deactivate()
        {
            _follower.gameObject.SetActive(false);
            _tipsManager.Star.SetActive(false);
            _tipsManager.CircleObjectPool.TurnOffAllPool();
            _isTracing = false;
        }
    }
}