using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    // ReSharper disable once InconsistentNaming

    public static class TTT
        {
        public const int ScoreCap = 10;

        private static Database _database;
        public static Database Database => _database ??= Resources.Load<Database>(nameof(Database));

        private static Breeding _breeding;
        public static Breeding Breeding => _breeding ??= Resources.Load<Breeding>(nameof(Breeding));
    }
}