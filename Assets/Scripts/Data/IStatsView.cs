using Drafts;
using TMPro;
using UnityEngine;

public class IStatsView : DataView<IStats> {
    [field: SerializeField] public TMP_Text Health { get; private set; }
    [field: SerializeField] public TMP_Text Shield { get; private set; }
    [field: SerializeField] public TMP_Text Mana { get; private set; }
    [field: SerializeField] public TMP_Text Speed { get; private set; }
    [field: SerializeField] public TMP_Text Hit { get; private set; }
    [field: SerializeField] public TMP_Text Evade { get; private set; }
    [field: SerializeField] public TMP_Text Armor { get; private set; }
    [field: SerializeField] public TMP_Text Resistence { get; private set; }
    [field: SerializeField] public TMP_Text Damage { get; private set; }
    [field: SerializeField] public TMP_Text CritChance { get; private set; }
    [field: SerializeField] public TMP_Text CritDamage { get; private set; }
    [field: SerializeField] public TMP_Text CastSpeed { get; private set; }

    protected override void Subscribe() {
        Health.TrySetText(Data[Stat.MaxHealth]);
        Shield.TrySetText(Data[Stat.MaxShield]);
        Mana.TrySetText(Data[Stat.MaxMana]);
        Speed.TrySetText(Data[Stat.Speed]);
        Hit.TrySetText(Data[Stat.Hit]);
        Evade.TrySetText(Data[Stat.Evade]);
        Armor.TrySetText(Data[Stat.Armor]);
        Resistence.TrySetText(Data[Stat.Resistance]);
        Damage.TrySetText(Data[Stat.Damage]);
        CritChance.TrySetText(Data[Stat.CritChance]);
        CritDamage.TrySetText(Data[Stat.CritDamage]);
        CastSpeed.TrySetText(Data[Stat.CastSpeed]);
    }

    protected override void Unsubscribe() {
        Health.TrySetText(null);
        Shield.TrySetText(null);
        Mana.TrySetText(null);
        Speed.TrySetText(null);
        Hit.TrySetText(null);
        Evade.TrySetText(null);
        Armor.TrySetText(null);
        Resistence.TrySetText(null);
        Damage.TrySetText(null);
        CritChance.TrySetText(null);
        CritDamage.TrySetText(null);
        CastSpeed.TrySetText(null);
    }
}