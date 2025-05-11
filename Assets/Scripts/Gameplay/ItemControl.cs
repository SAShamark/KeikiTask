using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ItemControl : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer[] _routePaths;

        [SerializeField]
        private Transform _follower;

        [SerializeField]
        private Camera _camera;

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

        private int _currentRouteIndex;
        private int _currentPointIndex;
        private bool _isTracing;

        private readonly List<Vector3> _routePoints = new();

        private void Start()
        {
            LoadRoute(_currentRouteIndex);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isTracing = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isTracing = false;
            }

            if (_isTracing && _currentPointIndex < _routePoints.Count)
            {
                Vector3 touchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                touchPos.z = 0f;

                float dist = Vector3.Distance(touchPos, _routePoints[_currentPointIndex]);
                if (dist <= _threshold)
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
                            Debug.Log("Всі маршрути пройдено!");
                            _follower.gameObject.SetActive(false);
                            _isTracing = false;
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

        private void LoadRoute(int index)
        {
            _routePoints.Clear();

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
    }
}