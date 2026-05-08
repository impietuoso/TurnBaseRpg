using System.Collections.Generic;
using Drafts;
using UnityEngine;
using UnityEngine.UI;

namespace TricksAndTreatsOrThreats.UI
{
    public class ScoreView : DataView<IScore>
    {
        [SerializeField] private DataView key;
        [SerializeField] private List<Image> dots;
        [SerializeField] private Sprite[] sprites;
        [SerializeField, Range(0, 25)] private int testValue;

        public IReadOnlyList<Sprite> SpritesOverride;

        private void OnEnable() => Redraw(Data?.Score ?? 0);

        protected override void Subscribe()
        {
            key.TrySetData(Data.Key);
            Redraw(Data?.Score ?? 0);
        }

        protected override void Unsubscribe()
        {
            throw new System.NotImplementedException();
        }

        private void Redraw(int value)
        {
            var icons = SpritesOverride ?? sprites;
            var loops = value / dots.Count;
            var rest = value % dots.Count;

            for (var i = 0; i < dots.Count; i++)
            {
                var index = loops + (i < rest ? 1 : 0);
                index = Mathf.Min(index, icons.Count - 1);
                dots[i].overrideSprite = icons[index];
            }
        }

        private void OnValidate() => Redraw(testValue);
    }
}