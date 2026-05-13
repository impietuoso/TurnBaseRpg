using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStatView : DataView<ResourceStat> {
    [SerializeField] private TMP_Text text;
    [SerializeField] private Slider bar;
    [SerializeField] private float animDuration = .25f;
    private string _format;

    private void Awake() {
        _format = text.text.Contains("{") ? text.text : "{0}";
    }

    protected override void Subscribe() {
        if (text) text.text = string.Format(_format, Data.Current, Data.Max);
        UpdateMax(null, 0);
        Data.OnChanged += UpdateCurrent;
        Data.OnMaxChanged += UpdateMax;
    }

    protected override void Unsubscribe() {
        Data.OnChanged -= UpdateCurrent;
        Data.OnMaxChanged -= UpdateMax;
    }

    private void UpdateMax(ResourceStat _, int __) {
        if (!bar) return;
        bar.maxValue = Data.Max;
        bar.value = Data.Current;
    }

    private void UpdateCurrent(ResourceStat _, int __) {
        if (text) text.text = string.Format(_format, Data.Current, Data.Max);
        StopAllCoroutines();
        if (bar) StartCoroutine(AnimateDamageTaken(Data.Current));
    }

    private IEnumerator AnimateDamageTaken(int targetValue) {
        var elapsed = 0f;
        var startValue = bar.value;

        while (elapsed < animDuration) {
            elapsed += Time.deltaTime;
            bar.value = Mathf.Lerp(startValue, targetValue, elapsed / animDuration);
            yield return null;
        }

        bar.value = targetValue;
    }
}