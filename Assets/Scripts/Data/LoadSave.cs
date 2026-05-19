using System.Collections.Generic;
using TricksAndTreatsOrThreats;
using UnityEngine;

public class LoadSave : MonoBehaviour {
    public SaveFile save;
    public List<CreatureSO> initialParty;
    public List<CreatureSO> availableCharacters;
    public ListInventory<InventoryItem> initialItens;
    public List<EnemyEncounter> encounters;
    public SaveView views;
    public ListView encounterListView;

    public void Awake() {
        encounterListView.SetData(encounters);
        
        if (PlayerPrefs.HasKey("SaveFile")) {
            var key = PlayerPrefs.GetString("SaveFile");
            save = JsonUtility.FromJson<SaveFile>(key);
        } else {
            save = new SaveFile {
                currentParty = new ObservableList<Creature>(),
                players = new ObservableList<Creature>(),
                inventory = initialItens
            };

            foreach (var so in initialParty) 
                save.currentParty.Add(Instantiate(so).Creature);

            while (save.currentParty.Count < 4) 
                save.currentParty.Add(null);

            foreach (var so in availableCharacters) 
                save.players.Add(Instantiate(so).Creature);

            while (save.players.Count < 8) 
                save.players.Add(null);

            views.SetData(save);
        }
    }
}