using UnityEngine;

public class StatsView : DataView<Stats> {
    [field: SerializeField] public BonusStatView Health { get; private set; }
    [field: SerializeField] public BonusStatView Shield { get; private set; }
    [field: SerializeField] public BonusStatView Mana { get; private set; }
    [field: SerializeField] public BonusStatView Speed { get; private set; }
    [field: SerializeField] public BonusStatView Hit { get; private set; }
    [field: SerializeField] public BonusStatView Evade { get; private set; }
    [field: SerializeField] public BonusStatView Armor { get; private set; }
    [field: SerializeField] public BonusStatView Resistence { get; private set; }
    [field: SerializeField] public BonusStatView Damage { get; private set; }
    [field: SerializeField] public BonusStatView CritChance { get; private set; }
    [field: SerializeField] public BonusStatView CritDamage { get; private set; }
    [field: SerializeField] public BonusStatView CastSpeed { get; private set; }

    protected override void Subscribe() {
        Health.SetData(Data[Stat.MaxHealth]);
        Shield.SetData(Data[Stat.MaxShield]);
        Mana.SetData(Data[Stat.MaxMana]);
        Speed.SetData(Data[Stat.Speed]);
        Hit.SetData(Data[Stat.Hit]);
        Evade.SetData(Data[Stat.Evade]);
        Armor.SetData(Data[Stat.Armor]);
        Resistence.SetData(Data[Stat.Resistance]);
        Damage.SetData(Data[Stat.Damage]);
        CritChance.SetData(Data[Stat.CritChance]);
        CritDamage.SetData(Data[Stat.CritDamage]);
        CastSpeed.SetData(Data[Stat.CastSpeed]);
    }

    protected override void Unsubscribe() {
        Health.SetData(null);
        Shield.SetData(null);
        Mana.SetData(null);
        Speed.SetData(null);
        Hit.SetData(null);
        Evade.SetData(null);
        Armor.SetData(null);
        Resistence.SetData(null);
        Damage.SetData(null);
        CritChance.SetData(null);
        CritDamage.SetData(null);
        CastSpeed.SetData(null);
    }
}