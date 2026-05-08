using Drafts;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    [CreateAssetMenu(menuName = "TTT/Race")]
    public class Race : DatabaseItem
    {
        [SerializeField] private Species species;
        [SerializeField] private ScoreList<Stat> stats;
        [SerializeField] private ScoreList<Activity> activities;
        [SerializeField] private int size;
        [SerializeField, Prefab] private Animator model;

        public Species Species => species;
        public IScoreList<Stat> Stats => stats;
        public IScoreList<Activity> Activities => activities;
        public int Size => size;
        public Animator Model => model;
    }
}