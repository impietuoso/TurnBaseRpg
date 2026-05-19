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
        [SerializeField] private FormationController formationController;
        [SerializeField] private ContextMenuView contextMenu;
        [SerializeField] private CallPopupText callPopup;

        [SerializeField] private float tickRate = .5f;
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
        private float _tickCounter;

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

        public Character Spawn(Creature creature, bool isAlly, Vector3 position) {
            var clone = Instantiate(characterPrefab, transform);
            clone.transform.position = position;
            clone.Spawn(this, creature, isAlly);

            _characters.Add(clone);
            if (isAlly) _allies.Add(clone);
            else _enemies.Add(clone);

            if (isAlly) clone.Pouch = playerInventory; //TODO

            return clone;
        }

        private void Update() {

            _tickCounter += Time.deltaTime;
            if (_tickCounter >= tickRate) {
                _tickCounter -= tickRate;
                foreach (var character in Characters) {
                    if (character.Health.Current <= 0) continue;
                    TickStatuses(character, tickRate);
                    ChargeAction(character, tickRate);
                    ApplyManaRegen(character, tickRate);
                }
            }

            HandleClicks();
        }

        private void ApplyManaRegen(Character character, float deltaTime) {
            if (character.Mana.Normalized >= 1) return;
            var amt = character.Mana.Max * manaRegenPercent * deltaTime;
            character.Mana.Current += (int)amt;
        }

        private void ChargeAction(Character character, float deltaTime) {
            if (character.InAction) return;

            if (character.NextAction != null) {
                character.NextAction.Charge(deltaTime);
                return;
            }

            if (enablePlayerBehaviour && character.isAlly)
                _enemyBehaviour.ChooseNextAction(this, character);
            if (enableEnemyBehaviour && !character.isAlly)
                _enemyBehaviour.ChooseNextAction(this, character);
        }

        private static void TickStatuses(Character character, float deltaTime) {
            foreach (var status in character.StatusEffectList.StatusList)
                status.Value.Tick(character, deltaTime);
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
                    formationController.MoveTo(_allies, hit.point);
            }
        }
    }
}