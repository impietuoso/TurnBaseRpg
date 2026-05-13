using Drafts;
using TricksAndTreatsOrThreats;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Passive")]
public class Passive : DatabaseItem {
    [SerializeReference, TypeInstance] private IPassive passive;
    public void Subscribe(Character target) => passive.Subscribe(target);
    public void Unsubscribe(Character target) => passive.Subscribe(target);
}