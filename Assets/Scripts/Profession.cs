using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Profession", fileName = "New Profession")]
public class Profession : ScriptableObject {
    [SerializeField] private Attributes initialStats;
    public Skill basicAttack;
    public List<Skill> starterSkills = new ();
    public List<Skill> unlockableSkills = new ();
    
    public IAttributes InitialStats => initialStats;
}