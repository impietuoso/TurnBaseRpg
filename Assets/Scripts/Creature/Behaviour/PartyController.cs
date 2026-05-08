using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TricksAndTreatsOrThreats.Behaviour
{
    // manage all creature behaviour
    public class PartyController : MonoBehaviour
    {
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private LayerMask creatureLayer;

        public CreatureBehaviour Target { get; private set; }
        public List<CreatureBehaviour> Party { get; private set; }

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            Party = FindObjectsByType<CreatureBehaviour>(FindObjectsSortMode.None).ToList();
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            // left click on creaturebehaviour: select (+shift: add to selection)
            if (mouse.leftButton.wasPressedThisFrame)
            {
                var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, creatureLayer))
                    Target = hit.collider.GetComponentInParent<CreatureBehaviour>();
                else Target = null;
            }

            // right click on floor: all selected MoveTo
            if (mouse.rightButton.wasPressedThisFrame)
            {
                var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, floorLayer))
                    foreach (var creature in Party)
                        creature.MoveTo(hit.point);
            }
        }
    }
}