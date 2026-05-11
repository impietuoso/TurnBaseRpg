using System.Collections;
using Drafts;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

public class PartyView : MonoBehaviour {
    [SerializeField] private CombatController controller;
    [SerializeField] private CollectionView party;
    [SerializeField] private CollectionView enemies;

    private IEnumerator Start() {
        yield return null;
        party.SetData(controller.Allies);
        enemies.SetData(controller.Enemies);
    }
}