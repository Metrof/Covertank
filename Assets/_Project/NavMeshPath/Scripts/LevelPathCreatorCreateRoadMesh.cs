using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelPathCreatorCreateRoadMesh : MonoBehaviour
{
    [SerializeField] private Material _roadMaterial;
    [SerializeField][Range(0, 1)] private float _pathSmooth = 0.2f;
    [SerializeField] private float _offsetForceOnTurn = 1;
    [SerializeField] private AnimationCurve _pathCurve;
    [SerializeField] private float _roadWidth = 1f;

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
    public void CreatePath()
    {
        _smoothedPath = SmoothPath(_path);
        DrawRoadMesh();
    }
    private List<Vector3> SmoothPath(Transform[] waypoints)
    {
        List<Vector3> smoothedPath = new List<Vector3>();

        if (waypoints.Length >= 2)
        {
            Vector3 previosMoveDirection = waypoints[1].position - waypoints[0].position;
            previosMoveDirection = previosMoveDirection.normalized;

            for (int i = 0; i < waypoints.Length - 1; i++)
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

    private void DrawRoadMesh()
    {
        if (_smoothedPath.Count < 2)
        {
            Debug.LogWarning("Для построения дороги необходимо минимум 2 точки.");
            return;
        }

        // Полный список вершин и треугольников
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Для каждой пары точек вычисляем сегмент дороги
        for (int i = 0; i < _smoothedPath.Count - 3; i++)
        {
            Vector3 start = _smoothedPath[i];
            Vector3 end = _smoothedPath[i + 1];

            //шов
            Vector3 seamStart = _smoothedPath[i + 2];
            Vector3 seamEnd = _smoothedPath[i + 3];

            Vector3 seamDirection = (seamEnd - seamStart).normalized;
            Vector3 seamSide = Vector3.Cross(seamDirection, Vector3.up) * _roadWidth / 2;

            // Вычисляем направление и боковые векторы
            Vector3 direction = (end - start).normalized;
            Vector3 side = Vector3.Cross(direction, Vector3.up) * _roadWidth / 2;

            // Добавляем вершины для сегмента
            int vertexIndex = vertices.Count;

            vertices.Add(start + side); // Левая вершина начала
            vertices.Add(start - side); // Правая вершина начала
            vertices.Add(seamStart + seamSide);   // Левая вершина конца
            vertices.Add(seamStart - seamSide);   // Правая вершина конца

            // Добавляем треугольники
            triangles.Add(vertexIndex + 0);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 1);

            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }

        // последняя интерация - start
        Vector3 lastStart = _smoothedPath[_smoothedPath.Count - 2];
        Vector3 lastEnd = _smoothedPath[_smoothedPath.Count - 1];

        Vector3 lastDirection = (lastEnd - lastStart).normalized;
        Vector3 lastSide = Vector3.Cross(lastDirection, Vector3.up) * _roadWidth / 2;

        int lVertexIndex = vertices.Count;

        vertices.Add(lastStart + lastSide); // Левая вершина начала
        vertices.Add(lastStart - lastSide); // Правая вершина начала
        vertices.Add(lastEnd + lastSide);   // Левая вершина конца
        vertices.Add(lastEnd - lastSide);   // Правая вершина конца

        // Добавляем треугольники
        triangles.Add(lVertexIndex + 0);
        triangles.Add(lVertexIndex + 2);
        triangles.Add(lVertexIndex + 1);

        triangles.Add(lVertexIndex + 1);
        triangles.Add(lVertexIndex + 2);
        triangles.Add(lVertexIndex + 3);

        // последняя интерация - end

        // Создаём Mesh
        Mesh roadMesh = new Mesh();
        roadMesh.vertices = vertices.ToArray();
        roadMesh.triangles = triangles.ToArray();
        roadMesh.RecalculateNormals();

        // Создаём объект дороги
        GameObject roadObject = new GameObject("Road");
        roadObject.transform.position = Vector3.zero + new Vector3(0, 0.1f, 0);
        roadObject.transform.parent = transform;

        MeshFilter meshFilter = roadObject.AddComponent<MeshFilter>();
        meshFilter.mesh = roadMesh;

        MeshRenderer meshRenderer = roadObject.AddComponent<MeshRenderer>();
        meshRenderer.material = _roadMaterial;

        if (string.IsNullOrEmpty(roadMesh.name))
            roadMesh.name = "FirstLvlRoadMesh";

        AssetDatabase.CreateAsset(roadMesh, "Assets/_Project/Environment/Lvls/1/FirstLvlRoadMesh.asset");
        AssetDatabase.SaveAssets();
    }
}

