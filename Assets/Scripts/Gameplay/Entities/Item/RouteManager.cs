using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Entities.Item
{
    [Serializable]
    public class RouteManager
    {
        [SerializeField]
        private float _samplingStep = 1f;

        internal List<Vector3> RoutePoints { get; private set; } = new();
        private TipsManager _tipsManager;

        public void Initialize(TipsManager tipsManager)
        {
            _tipsManager = tipsManager;
        }

        internal void ProcessRoute(LineRenderer route)
        {
            RoutePoints.Clear();

            int segmentCount = route.positionCount - 1;

            for (int i = 0; i < segmentCount; i++)
            {
                Vector3 start = route.GetPosition(i);
                Vector3 end = route.GetPosition(i + 1);
                CreateSegmentPoints(start, end, i == segmentCount - 1);
            }
        }

        private void CreateSegmentPoints(Vector3 start, Vector3 end, bool isFinalSegment)
        {
            float segmentLength = Vector3.Distance(start, end);
            int steps = Mathf.CeilToInt(segmentLength / _samplingStep);

            for (int j = 0; j <= steps; j++)
            {
                Vector3 point = CalculatePoint(start, end, j, steps);
                RoutePoints.Add(point);

                bool isLastPoint = isFinalSegment && j == steps;

                _tipsManager.HandlePointVisualization(isLastPoint, point);
            }
        }

        private Vector3 CalculatePoint(Vector3 start, Vector3 end, int step, int totalSteps)
        {
            float t = (float)step / totalSteps;
            Vector3 point = Vector3.Lerp(start, end, t);
            point.z = 0f;
            return point;
        }
    }
}