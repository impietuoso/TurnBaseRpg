using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    public interface IScoreList<T> : IEnumerable<IScore<T>>
    {
        int this[T stat] { get; }
    }

    [Serializable]
    public class ScoreList<T> : IScoreList<T> where T : class
    {
        public ScoreList() => values = new();

        public ScoreList([NotNull] IEnumerable<IScore<T>> other)
            => values = other.Select(s => new ScoreT<T>(s.Key, s.Score)).ToList();

        [SerializeField] private List<ScoreT<T>> values;

        public int Count => values.Count;

        public int this[T stat]
        {
            get { return values.FirstOrDefault(v => v.Key == stat)?.score ?? 0; }
            set
            {
                var index = values.FindIndex(s => s.Key == stat);
                if (index < 0)
                {
                    if (value > 0)
                        values.Add(new ScoreT<T>(stat, value));
                    return;
                }

                if (value <= 0)
                {
                    values.RemoveAt(index);
                    return;
                }

                values[index].score = Mathf.Min(TTT.ScoreCap, value);
            }
        }

        public IEnumerator<IScore<T>> GetEnumerator() => values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();
    }
}