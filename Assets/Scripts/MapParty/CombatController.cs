using System;
using System.Collections.Generic;
using Drafts;
using TTT.ContextMenus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TricksAndTreatsOrThreats.Behaviour {
    // manage all creature behaviour

    public class CombatController : MonoBehaviour {
        public static CombatController Instance { get; private set; }

        [SerializeField, Prefab] private Character characterPrefab;
        [SerializeField] private Skill basicDefendSkill;
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private LayerMask creatureLayer;
        [SerializeField] private ContextMenuView contextMenu;
        [SerializeField] private CallPopupText callPopup;

        [SerializeField] private float maxActionPoints = 10f;
        [SerializeField] private float manaRegenPercent = .1f;
        [SerializeField] private bool enablePlayerBehaviour;
        [SerializeField] private bool enableEnemyBehaviour;

        public IReadOnlyCollection<Character> Characters => _characters;
        public IObservableList<Character> Allies => _allies;
        public IReadOnlyList<Character> Enemies => _enemies;
        public List<object> TaskList { get; } = new ();
        public bool IsBusy => TaskList.Count > 0;
        public CallPopupText CallPopup => callPopup;
        public Skill BasicDefendSkill => basicDefendSkill;

        [Obsolete] public ListInventory<Consumable> playerInventory;

        private readonly EnemyBehaviour _enemyBehaviour = new ();
        private readonly HashSet<Character> _characters = new ();
        private readonly ObservableList<Character> _allies = new ();
        private readonly List<Character> _enemies = new ();

        private Camera _camera;

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

        public Character Spawn(PartyMember member, bool isAlly, Vector3 position) {
            var clone = Instantiate(characterPrefab, transform);
            clone.transform.position = position;
            clone.Initialize(this, member, isAlly);

            _characters.Add(clone);
            if (isAlly) _allies.Add(clone);
            else _enemies.Add(clone);

            if (isAlly) clone.Pouch = playerInventory; //TODO

            return clone;
        }

        private void Update() {
            foreach (var character in Characters) {
                TickStatuses(character);
                ChargeAction(character);
                ApplyManaRegen(character);
            }

            HandleClicks();
        }

        private void ApplyManaRegen(Character character) {
            var amt = character.derivedStats.mana.maxValue * manaRegenPercent;
            character.derivedStats.mana.AddClampedBaseValue((int)amt);
        }

        private void ChargeAction(Character character) {
            if (character.InAction) return;
            if (character.derivedStats.health.currentValue <= 0) return;

            if (character.NextAction != null) {
                character.NextAction.Charge(Time.deltaTime);
                return;
            }

            if (enablePlayerBehaviour && character.isAlly)
                _enemyBehaviour.ChooseNextAction(this, character);
            if (enableEnemyBehaviour && !character.isAlly)
                _enemyBehaviour.ChooseNextAction(this, character);
        }

        private static void TickStatuses(Character character) {
            foreach (var status in character.StatusEffectList.StatusList)
                status.Value.Tick(character, Time.deltaTime);
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
                    var target = hit.collider.GetComponentInParent<Character>();
                    if (target) {
                        contextMenu.gameObject.SetActive(true);
                        contextMenu.SetData(target.Menu);
                    }
                }
            }

            // right click on floor: all selected MoveTo
            if (mouse.leftButton.wasPressedThisFrame) {
                var ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, floorLayer))
                    foreach (var creature in _allies)
                        creature.MoveTo(hit.point);
            }
        }
    }
}