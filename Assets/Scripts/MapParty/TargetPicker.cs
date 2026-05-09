using UnityEngine;
using UnityEngine.InputSystem;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class TargetPicker : MonoBehaviour
    {
        public static TargetPicker Instance { get; private set; }

        [SerializeField] private LineRenderer line;
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private int resolution = 20;
        [SerializeField] private float arcHeight = 2f;

        private Transform _source;
        private Camera _camera;

        private void Start()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _camera = Camera.main;
            line.enabled = false;
            gameObject.SetActive(false);
        }

        public void ShowArrow(Transform src, Color color)
        {
            _source = src;
            line.startColor = color;
            line.endColor = color;
            line.enabled = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                gameObject.SetActive(false);
                return;
            }

            var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
            Vector3 targetPos;

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, floorLayer))
                targetPos = hit.point;
            else
                targetPos = ray.GetPoint(10f);

            DrawBezier(targetPos);
        }

        private void DrawBezier(Vector3 targetPos)
        {
            line.positionCount = resolution;
            var startPos = _source.position;

            // Midpoint with an offset for the arc
            var midPoint = Vector3.Lerp(startPos, targetPos, 0.5f);
            midPoint.y += arcHeight;

            for (var i = 0; i < resolution; i++)
            {
                var t = i / (float)(resolution - 1);
                var point = CalculateQuadraticBezierPoint(t, startPos, midPoint, targetPos);
                line.SetPosition(i, point);
            }
        }

        private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            // B(t) = (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
            return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
        }
    }
}