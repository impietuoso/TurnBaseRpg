using System;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    public interface IScore
    {
        object Key { get; }
        int Score { get; }
    }

    public interface IScore<out T> : IScore
    {
        new T Key { get; }
        object IScore.Key => Key;
    }

    [Serializable]
    public class ScoreT<T> : IScore<T>
    {
        [SerializeField] private T key;
        [Range(0, 25)] public int score = 1;

        public ScoreT() { }

        public ScoreT(T key, int score = 1)
        {
            this.key = key;
            this.score = score;
        }

        public T Key => key;
        public int Score => score;
        public static implicit operator ScoreT<T>((T k, int s) pair) => new(pair.k, pair.s);
        public override string ToString() => $"{{{key}, {score}}}";
    }
}