using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class TargetPicker : MonoBehaviour
    {
        public static TargetPicker Instance { get; private set; }

        [SerializeField] private LineRenderer line;
        [SerializeField] private SpriteRenderer targetMarker;
        [SerializeField] private Image icon;
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private LayerMask creatureLayer;
        [SerializeField] private Vector2 iconOffset;
        [SerializeField] private int resolution = 20;
        [SerializeField] private float arcHeight = 2f;

        private TargetPickerArgs _args;
        private Camera _camera;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;
            line.enabled = false;
            gameObject.SetActive(false);
        }

        public void ShowArrow(TargetPickerArgs args)
        {
            _args = args;
            line.startColor = args.ArrowColor;
            line.endColor = args.ArrowColor;
            line.enabled = true;
            icon.sprite = args.Icon;
            icon.enabled = true;
            gameObject.SetActive(true);
            Update();
        }

        private void Update()
        {
            if (_args == null) return;
            var mouse = Mouse.current;
            if (mouse == null) return;

            if (mouse.rightButton.wasPressedThisFrame)
            {
                _args = null;
                gameObject.SetActive(false);
                return;
            }

            var mousePos = mouse.position.ReadValue();
            var ray = _camera.ScreenPointToRay(mousePos);
            Vector3 targetPos;
            Character target = null;

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, creatureLayer | floorLayer))
            {
                target = hit.collider.GetComponentInParent<Character>();
                if (target && !_args.Validate(target)) target = null;
                targetPos = target ? target.Position : hit.point;
            }
            else
                targetPos = ray.GetPoint(10f);

            DrawBezierArrow(targetPos);
            DrawTargetMarker(target);
            icon.transform.position = mousePos + iconOffset;

            if (!mouse.leftButton.wasPressedThisFrame || !target) return;
            if (!_args.Validate(target)) return;
            _args.Confirm(target);
            _args = null;
            gameObject.SetActive(false);
        }

        private void DrawTargetMarker(Character target)
        {
            if (!target)
            {
                targetMarker.enabled = false;
                return;
            }

            var pos = targetMarker.transform.position;
            pos.x = target.Position.x;
            pos.z = target.Position.z;
            targetMarker.enabled = target;
            targetMarker.transform.position = pos;
        }

        private void DrawBezierArrow(Vector3 targetPos)
        {
            line.positionCount = resolution;
            var startPos = _args.User.Position;

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