using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPathCreator : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float _pathSmooth = 0.2f;
    [SerializeField] private float _offsetForceOnTurn = 1;
    [SerializeField] private AnimationCurve _pathCurve;

    private ChankNavPoints[] _chankNavPoints;
    private Transform[] _path = new Transform[0];
    private List<Vector3> _smoothedPath;
    private void Awake()
    {
        _chankNavPoints = GetComponentsInChildren<ChankNavPoints>();
        foreach (var points in _chankNavPoints)
        {
            _path = _path.Concat(points.Points).ToArray();
        }
    }
    public List<Vector3> GetPath()
    {
        return _smoothedPath;
    }
    public void CreatePath(Vector3 lastPoint)
    {
        _smoothedPath = SmoothPath(_path);
        _smoothedPath.Add(lastPoint);
    }
    private List<Vector3> SmoothPath(Transform[] waypoints)
    {
        List<Vector3> smoothedPath = new List<Vector3>();

        if (waypoints.Length >= 2)
        {
            Vector3 previosMoveDirection = waypoints[1].position - waypoints[0].position;
            previosMoveDirection = previosMoveDirection.normalized;

            for (int i = 0; i < waypoints.Length - 1; i ++)
            {
                Vector3 firstPoint = waypoints[i].position;
                Vector3 secondPoint = waypoints[i + 1].position;

                Vector3 offsetPoint = previosMoveDirection * _offsetForceOnTurn + firstPoint;

                // Добавляем промежуточные точки
                for (float t = 0; t <= 1; t += _pathSmooth)
                {
                    float d = _pathCurve.Evaluate(t);

                    Vector3 firstLerpPos = Vector3.Lerp(firstPoint, offsetPoint, d);
                    Vector3 secondLerpPos = Vector3.Lerp(offsetPoint, secondPoint, t);

                    smoothedPath.Add(Vector3.Lerp(firstLerpPos, secondLerpPos, t));
                }
                previosMoveDirection = secondPoint - firstPoint;
                previosMoveDirection = previosMoveDirection.normalized;
            }
            smoothedPath.Add(waypoints[waypoints.Length - 1].position);
        }
        else
        {
            smoothedPath.Add(waypoints[0].position);
        }

        return smoothedPath;
    }
}
