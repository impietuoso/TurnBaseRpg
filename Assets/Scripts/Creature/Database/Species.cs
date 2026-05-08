using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    [CreateAssetMenu(menuName = "TTT/Species")]
    public class Species : DatabaseItem
    {
        [SerializeField] private List<Race> races;
        private ScoreList<Stat> _stats;
        private ScoreList<Activity> _activities;

        public IReadOnlyList<Race> Races => races ??= GetRaces(this);

        public IScoreList<Stat> Stats => _stats ??= Races.Aggregate(
            new ScoreList<Stat>(), (result, r) =>
            {
                foreach (var pair in r.Stats)
                    if (pair.Score > result[pair.Key])
                        result[pair.Key] = pair.Score;
                return result;
            });

        public IScoreList<Activity> Activities => _activities ??= Races.Aggregate(
            new ScoreList<Activity>(), (result, r) =>
            {
                foreach (var pair in r.Activities)
                    if (pair.Score > result[pair.Key])
                        result[pair.Key] = pair.Score;
                return result;
            });

        public static List<Race> GetRaces(Species caller)
        {
            foreach (var s in TTT.Database.GetAll<Species>())
            {
                s.races = new();
                s._stats = null;
                s._activities = null;
            }

            foreach (var r in TTT.Database.GetAll<Race>()) r.Species.races.Add(r);
            return caller?.races;
        }
    }
}