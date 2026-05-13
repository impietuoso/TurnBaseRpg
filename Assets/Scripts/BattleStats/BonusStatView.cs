using TMPro;
using UnityEngine;

public class BonusStatView : DataView<BonusStat> {
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color buffColor = Color.green;
    [SerializeField] private Color debuffColor = Color.red;
    private Color _color;

    private void Awake() => _color = text.color;

    protected override void Subscribe() {
        Data.OnChanged += StatValueChanged;
        StatValueChanged(Data.Total);
    }

    protected override void Unsubscribe() {
        Data.OnChanged -= StatValueChanged;
    }

    private void StatValueChanged(int v) {
        text.text = v.ToString();
        text.color = v == Data.Base ? _color : v > Data.Base ? debuffColor : buffColor;
    }
}