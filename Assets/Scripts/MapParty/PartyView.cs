using Drafts;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

public class PartyView : MonoBehaviour {
    [SerializeField] private CombatController controller;
    [SerializeField] private CollectionView party;

    private void Start() {
        party.SetData(controller.Allies);
    }
}