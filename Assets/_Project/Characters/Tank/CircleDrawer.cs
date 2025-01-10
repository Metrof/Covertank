using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour
{
    [SerializeField] private int _segments = 50; // Количество сегментов (точек на круге)

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawCircle(float radius)
    {
        // Настройка LineRenderer
        lineRenderer.positionCount = _segments + 1; // +1 для замыкания круга
        lineRenderer.useWorldSpace = false;       // Относительные координаты
        lineRenderer.loop = true;                 // Замкнуть линию

        // Рассчёт позиций точек
        float angleStep = 360f / _segments; // Угол между точками
        for (int i = 0; i <= _segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0, y));
        }
    }
}
