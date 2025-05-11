using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Audio.Data;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ItemControl : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer[] _routePaths;

        [SerializeField]
        private Transform _follower;

        [SerializeField]
        private float _threshold = 0.5f;

        [SerializeField]
        private float _followSpeed = 10f;

        [SerializeField]
        private float _samplingStep = 0.1f;

        [SerializeField]
        private GameObject _circlePrefab;

        [SerializeField]
        private GameObject _starPrefab;

        [SerializeField]
        private Transform _routeVisualsParent;

        [SerializeField]
        private Transform _handTip;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private Camera _camera;
        private int _currentRouteIndex;
        private int _currentPointIndex;
        private bool _isTracing;
        private readonly List<Vector3> _routePoints = new();
        private IAudioManager _audioManager;
        private Coroutine _audioCoroutine;
        private Coroutine _afkCoroutine;
        private string _soundName;

        public event Action OnLevelCompleted;

        private void OnDestroy()
        {
            if (_audioCoroutine != null)
            {
                StopCoroutine(_audioCoroutine);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_afkCoroutine != null)
                {
                    StopCoroutine(_afkCoroutine);
                }

                _handTip.gameObject.SetActive(false);

                _isTracing = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _afkCoroutine = StartCoroutine(AfkManage());
                _isTracing = false;
            }

            if (_isTracing && _currentPointIndex < _routePoints.Count)
            {
                Vector3 touchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0f;

                float distance = Vector3.Distance(touchPosition, _routePoints[_currentPointIndex]);
                if (distance <= _threshold)
                {
                    _currentPointIndex++;

                    if (_currentPointIndex >= _routePoints.Count)
                    {
                        _currentRouteIndex++;
                        if (_currentRouteIndex < _routePaths.Length)
                        {
                            LoadRoute(_currentRouteIndex);
                        }
                        else
                        {
                            _follower.gameObject.SetActive(false);
                            _isTracing = false;

                            string[] soundNames =
                                { SoundsConstants.AWESOME, SoundsConstants.EXCELLENT, SoundsConstants.THATS_GOOD };
                            string randomSound = soundNames[Random.Range(0, soundNames.Length)];
                            StartCoroutine(PlayAudioThenLoad(randomSound, LevelCompleted));
                        }
                    }
                }

                if (_currentPointIndex < _routePoints.Count)
                {
                    _follower.position = Vector3.MoveTowards(_follower.position, _routePoints[_currentPointIndex],
                        _followSpeed * Time.deltaTime);
                }
            }
        }

        public void Initialize(IAudioManager audioManager, Camera camera, Color color, string soundName)
        {
            _camera = camera;
            _audioManager = audioManager;
            _spriteRenderer.color = color;
            _soundName = soundName;

            _audioCoroutine = StartCoroutine(PlayAudioThenLoad(soundName, () => LoadRoute(_currentRouteIndex)));
            _afkCoroutine = StartCoroutine(AfkManage());
        }

        private IEnumerator PlayAudioThenLoad(string audioName, Action action)
        {
            AudioSource source = _audioManager.Play(AudioGroupType.EffectSounds, audioName);
            yield return new WaitWhile(() => source.isPlaying);
            action?.Invoke();
        }

        private IEnumerator AfkManage()
        {
            yield return new WaitForSeconds(7);
            _audioManager.Play(AudioGroupType.EffectSounds, _soundName);
            yield return new WaitForSeconds(7);
            if (_routePoints.Count == 0)
            {
                yield break;
            }

            _handTip.position = _follower.position;
            _handTip.gameObject.SetActive(true);

            float durationPerSegment = 0.5f;

            for (int i = _currentPointIndex; i < _routePoints.Count; i++)
            {
                Vector3 target = _routePoints[i];
                Tween moveTween = _handTip.DOMove(target, durationPerSegment).SetEase(Ease.Linear);
                yield return moveTween.WaitForCompletion();
            }

            _handTip.gameObject.SetActive(false);
        }

        private void LoadRoute(int index)
        {
            _routePoints.Clear();
            _follower.gameObject.SetActive(true);
            foreach (Transform child in _routeVisualsParent)
                Destroy(child.gameObject);

            if (index >= _routePaths.Length) return;

            LineRenderer route = _routePaths[index];
            int pointCount = route.positionCount;

            for (int i = 0; i < pointCount - 1; i++)
            {
                Vector3 start = route.GetPosition(i);
                Vector3 end = route.GetPosition(i + 1);

                float distance = Vector3.Distance(start, end);
                int segments = Mathf.CeilToInt(distance / _samplingStep);

                for (int j = 0; j <= segments; j++)
                {
                    float t = j / (float)segments;
                    Vector3 point = Vector3.Lerp(start, end, t);
                    point.z = 0f;
                    _routePoints.Add(point);

                    GameObject prefabToSpawn = _circlePrefab;

                    if (i == pointCount - 2 && j == segments)
                    {
                        prefabToSpawn = _starPrefab;
                    }

                    Instantiate(prefabToSpawn, point, Quaternion.identity, _routeVisualsParent);
                }
            }

            _currentPointIndex = 0;
            if (_routePoints.Count > 0)
                _follower.position = _routePoints[0];
        }

        private void LevelCompleted()
        {
            OnLevelCompleted?.Invoke();
        }
    }
}