public class SaveView : DataView<SaveFile> {
    public ListView partyView;
    public ListView availableView;
    public IInventoryView inventoryView;

    protected override void Subscribe() {
        if (partyView) partyView.SetData(Data.currentParty);
        if (availableView) availableView.SetData(Data.players);
        if (inventoryView) inventoryView.SetData(Data.inventory);
    }

    protected override void Unsubscribe() {
        if (partyView) partyView.SetData(null);
        if (availableView) availableView.SetData(null);
        if (inventoryView) inventoryView.SetData(null);
    }
}
