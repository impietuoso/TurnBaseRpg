using System.Collections.Generic;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class Billboarding : MonoBehaviour
    {
        public static HashSet<Billboarding> Instances { get; } = new();

        [SerializeField] private Vector3 angleOffset;
        private Camera _mainCamera;

        private void Awake() => Instances.Add(this);
        private void OnDestroy() => Instances.Remove(this);

        private void Start()
        {
            _mainCamera = Camera.main;
            UpdateAngle();
        }

        public void UpdateAngle()
        {
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                _mainCamera.transform.rotation * Vector3.up);
            transform.Rotate(angleOffset, Space.Self);
        }
    }
}