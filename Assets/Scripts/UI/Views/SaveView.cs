public class SaveView : DataView<SaveFile> {
    public ListView partyView;
    public ListView availableView;
    public IInventoryView inventoryView;

    public override void Subscribe() {
        if (partyView) partyView.SetData(data.currentParty);
        if (availableView) availableView.SetData(data.players);
        if (inventoryView) inventoryView.SetData(data.inventory);
    }

    public override void Unsubscribe() {
        if (partyView) partyView.SetData(null);
        if (availableView) availableView.SetData(null);
        if (inventoryView) inventoryView.SetData(null);
    }
}
