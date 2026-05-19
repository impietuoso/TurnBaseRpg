using System;
using System.Collections.Generic;
using System.Linq;
using Drafts;
using UnityEngine;

namespace TricksAndTreatsOrThreats {
    [Serializable]
    public class Creature : IAttributes, IStats {
        public Creature() { }

        public Creature(Race race, Attributes attributes) {
            this.race = race;
            bornAttributes = attributes;
        }

        [SerializeField] public string nickname;
        [SerializeField] public int level;
        [SerializeField] private Race race;
        [Separator]
        [SerializeField] private Attributes bornAttributes;
        [SerializeField] private Attributes levelAttributes = new ();
        [SerializeField] private ObservableList<Skill> skills = new ();
        [SerializeField] private ObservableList<Passive> passives = new ();
        [SerializeField, EquipArray] private ObservableList<Equipment> equips = new ();

        public string DisplayName => string.IsNullOrEmpty(nickname) ? Race.DisplayName : nickname;
        public Race Race => race;
        public IScoreList<Activity> Activities => race.Activities;
        public IAttributes BornAttributes => bornAttributes;
        public Attributes LevelAttributes => levelAttributes;
        public ObservableList<Skill> Skills => skills;
        public ObservableList<Passive> Passives => passives;
        public ObservableList<Equipment> Equips => equips;
       [ThreeColumns] public List<Equipment> list;

        public int this[Attribute a] => bornAttributes[a] + LevelAttributes[a];
        public int this[Stat s] => Equips.Sum(i => i[s]);

        public int GetUnusedPoints() => level / 2 - levelAttributes.Sum();
    }
}