using UnityEngine;

public interface ITarget
{
    Vector3 Position { get; }
    bool IsValid { get; }
}

public class TargetPosition : ITarget
{
    public bool IsValid => true;
    public Vector3 Position { get; set; }
}