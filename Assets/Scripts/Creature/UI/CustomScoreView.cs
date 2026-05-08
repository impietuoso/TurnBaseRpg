using System.Collections.Generic;
using UnityEngine;

namespace TricksAndTreatsOrThreats.UI
{
    public class CustomScoreView : DataView<(IScore, IReadOnlyList<Sprite>)>
    {
        [SerializeField] private ScoreView score;

        protected override void Subscribe()
        {
            var (scores, sprites) = Data;
            score.SpritesOverride = sprites;
            score.SetData(scores);
        }

        protected override void Unsubscribe()
        {
            score.SpritesOverride = null;
            score.SetData(null);
        }
    }
}