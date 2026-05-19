using System;
using Drafts;
using UnityEngine;

namespace TricksAndTreatsOrThreats {
    [CreateAssetMenu(menuName = "TTT/Race")]
    public class Race : DatabaseItem {
        [SerializeField] private Species species;
        [SerializeField] private ScoreList<BonusStat> stats;
        [SerializeField] private ScoreList<Activity> activities;
        [SerializeField] private int size;
        [SerializeField, Prefab] private Animator model;
        [Separator]
        [SerializeField, Prefab] private Sprite sprite;

        public Species Species => species;
        public IScoreList<BonusStat> Stats => stats;
        public IScoreList<Activity> Activities => activities;
        public int Size => size;
        public Sprite Sprite => sprite;
        [Obsolete] public Animator Model => model;
    }
}