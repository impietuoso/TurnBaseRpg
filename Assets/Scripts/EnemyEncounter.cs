using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/EnemyEncounter", fileName = "New Enemy Encounter")]
public class EnemyEncounter : ScriptableObject {
    public string encounterName;
    public List<PartyMember> enemyList;
}