using UnityEngine;

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    private static T _instance;
    public static T Instance => _instance ??= Resources.Load<T>(typeof(T).Name);
}