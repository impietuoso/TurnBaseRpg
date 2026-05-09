using TricksAndTreatsOrThreats;
using UnityEngine;

public static class Game
{
    private static GameSettings _settings;
    private static Database _database;

    public static GameSettings Settings => _settings ??= Resources.Load<GameSettings>(nameof(GameSettings));
    public static Database Database => _database ??= Resources.Load<Database>(nameof(Database));

    //Inventário dos Personagens
    //Sistema de Level up
    //Sistema de Montagem de PT
    //Sistema de Seleção de Skills para personagens
}

[CreateAssetMenu(menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
   
}