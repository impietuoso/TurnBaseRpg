using System.Collections.Generic;
using TricksAndTreatsOrThreats;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/EnemyEncounter", fileName = "New Enemy Encounter")]
public class EnemyEncounter : ScriptableObject {
    public string encounterName;
    public List<Creature> enemyList;
}