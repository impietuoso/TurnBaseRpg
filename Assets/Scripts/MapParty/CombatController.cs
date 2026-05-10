using System.Collections.Generic;
using System.Linq;
using Drafts;
using TTT.ContextMenus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TricksAndTreatsOrThreats.Behaviour {
    // manage all creature behaviour
    public class CombatController : MonoBehaviour {
        public static CombatController Instance { get; private set; }

        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private LayerMask creatureLayer;
        [SerializeField] private ContextMenuView contextMenu;
        [SerializeField, Prefab] private Character characterPrefab;
        [SerializeField] private float maxActionPoints = 10f;
        [SerializeField] private float manaRegenPercent = .1f;

        public IReadOnlyCollection<Character> Characters => _characters;
        public IEnumerable<Character> GetTeam(string team) => _teams.TryGetValue(team, out var t) ? t : Enumerable.Empty<Character>();
        public Character Target { get; private set; }
        public List<object> TaskList { get; } = new ();
        public bool IsBusy => TaskList.Count > 0;
        public float MaxActionPoints => maxActionPoints;

        private readonly HashSet<Character> _characters = new ();
        private readonly Dictionary<string, List<Character>> _teams = new ();
        private Camera _camera;
        public CallPopupText callPopup;

        private void Awake() {
            if (Instance) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start() {
            _camera = Camera.main;
            contextMenu.gameObject.SetActive(false);
        }

        public Character Spawn(PartyMember member, string team, Vector3 position) {
            var clone = Instantiate(characterPrefab, transform);
            clone.transform.position = position;
            clone.Initialize(this, member, team);
            if (!_teams.TryGetValue(team, out var t))
                _teams[team] = t = new ();
            t.Add(clone);
            _characters.Add(clone);

            return clone;
        }

        private void Update() {
            foreach (var team in _teams.Values)
            foreach (var character in team) {
                TickStatuses(character);
                UpdateActionPoints(character);
                ApplyManaRegen(character);
            }

            HandleClicks();
        }

        private void ApplyManaRegen(Character character) {
            var amt = character.derivedStats.mana.maxValue * manaRegenPercent;
            character.derivedStats.mana.AddClampedBaseValue((int)amt);
        }

        private void UpdateActionPoints(Character newChar) {
            if (newChar.derivedStats.health.currentValue <= 0) return;
            newChar.actionPoints.Value += newChar.derivedStats.speed.currentValue * Time.deltaTime;

            if (newChar.actionPoints.Value >= maxActionPoints) {
                newChar.actionPoints.Value -= maxActionPoints;
                //TODO Take Action
            }
        }

        private static void TickStatuses(Character character) {
            foreach (var status in character.StatusEffectList.StatusList)
                ; //TODO status.Value.Tick(Time.deltaTime);
        }

        private void HandleClicks() {
            if (IsBusy) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var mouse = Mouse.current;
            if (mouse == null) return;

            // left click on creaturebehaviour: select (+shift: add to selection)
            if (mouse.rightButton.wasPressedThisFrame) {
                var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, creatureLayer)) {
                    Target = hit.collider.GetComponentInParent<Character>();
                    if (Target) {
                        contextMenu.gameObject.SetActive(true);
                        contextMenu.SetData(Target.Menu);
                    }
                } else Target = null;
            }

            // right click on floor: all selected MoveTo
            if (mouse.leftButton.wasPressedThisFrame) {
                var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, floorLayer))
                    foreach (var creature in _teams["player"])
                        creature.MoveTo(hit.point);
            }
        }
    }
}