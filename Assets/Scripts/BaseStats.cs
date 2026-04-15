using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BaseStats : IEnumerable<int> {
    public int strength = 5;
    public int intelligence = 5;
    public int dexterity = 5;
    public int vitality = 5;
    public int spirit = 5;

    public int this[StatName statName] {
        get {
            return statName switch {
                StatName.strength => strength,
                StatName.intelligence => intelligence,
                StatName.dexterity => dexterity,
                StatName.vitality => vitality,
                StatName.spirit => spirit,
                _ => 0,
            };
        } set {
            switch (statName) {
                case StatName.strength: strength = value; break;
                case StatName.intelligence: intelligence = value; break;
                case StatName.dexterity: dexterity = value; break;
                case StatName.vitality: vitality = value; break;
                case StatName.spirit: spirit = value; break;
            }
        }
    }
    public static BaseStats operator +(BaseStats a, BaseStats b) {
        return new BaseStats {
            strength = a.strength + b.strength,
            intelligence = a.intelligence + b.intelligence,
            dexterity = a.dexterity + b.dexterity,
            vitality = a.vitality + b.vitality,
            spirit = a.spirit + b.spirit
        };
    }
    public IEnumerator<int> GetEnumerator() {
        yield return strength;
        yield return intelligence;
        yield return dexterity;
        yield return vitality;
        yield return spirit;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
public enum StatName { strength, intelligence, dexterity, vitality, spirit }