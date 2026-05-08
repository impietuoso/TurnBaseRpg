using System.Collections.Generic;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    [CreateAssetMenu(menuName = "TTT/Activity")]
    public class Activity : DatabaseItem
    {
        private IReadOnlyList<Sprite> _icons;
        public IReadOnlyList<Sprite> Icons => _icons ?? new[] { null, Icon };
    }
}