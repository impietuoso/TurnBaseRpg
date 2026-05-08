using System;

namespace TricksAndTreatsOrThreats
{
    [Serializable]
    public class Creature
    {
        public string nickname;
        public Race race;
        public ScoreList<Stat> stats;
        public ScoreList<Activity> activities;
        public Anatomy anatomy;

        public string DisplayName => string.IsNullOrEmpty(nickname) ? race.DisplayName : nickname;
    }

    [Serializable]
    public class Anatomy
    {
        public float height;
        public float length;
        public float weight;
    }
}