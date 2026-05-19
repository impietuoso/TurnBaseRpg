using Drafts;
using TricksAndTreatsOrThreats;
using UnityEngine;

public static class Game {
    private static GameSettings _settings;
    private static Database _database;

    public static GameSettings Settings => _settings ??= Resources.Load<GameSettings>(nameof(GameSettings));
    public static Database Database => _database ??= Resources.Load<Database>(nameof(Database));

    public static EquipSlot[] EquipmentOrder { get; } = {
        EquipSlot.Infusion,
        EquipSlot.Core,
        EquipSlot.Misc,
        EquipSlot.Misc
    };

    [RuntimeInitializeOnLoadMethod]
    private static void Init() {
        TypeCache.SetAssemblies(typeof(Game).Assembly);
        Database.SetRuntimeIndexes();
    }
}