using UnityEngine;

[CreateAssetMenu(menuName = "Game/ContextMenuAssets", fileName = "ContextMenuAssets")]
public class ContextMenuAssets : ScriptableSingleton<ContextMenuAssets>
{
    [field: SerializeField] public Sprite AttackIcon { get; private set; }
    [field: SerializeField] public Color AttackColor { get; private set; }
    [field: SerializeField] public Sprite DefendIcon { get; private set; }
    [field: SerializeField] public Color DefendColor { get; private set; }
    [field: SerializeField] public Sprite SkillIcon { get; private set; }
    [field: SerializeField] public Color SkillColor { get; private set; }
    [field: SerializeField] public Sprite ItemIcon { get; private set; }
    [field: SerializeField] public Color ItemColor { get; private set; }
    [field: SerializeField] public Sprite BackIcon { get; private set; }
    [field: SerializeField] public Color BackColor { get; private set; }
}