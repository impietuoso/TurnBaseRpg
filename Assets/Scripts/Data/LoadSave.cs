using System.Collections.Generic;
using UnityEngine;

public class LoadSave : MonoBehaviour {
    public SaveFile save;
    public List<PartyMember> initialParty;
    public List<PartyMember> availableCharacters;
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
                currentParty = new ObservableList<PartyMember>(),
                players = new ObservableList<PartyMember>(),
                inventory = initialItens
            };

            foreach (var character in initialParty) {
                save.currentParty.Add(Instantiate(character));
            }

            while (save.currentParty.Count < 4) {
                save.currentParty.Add(null);
            }

            foreach (var character in availableCharacters) {
                save.players.Add(Instantiate(character));
            }

            while (save.players.Count < 8) {
                save.players.Add(null);
            }

            views.SetData(save);
        }
    }
}