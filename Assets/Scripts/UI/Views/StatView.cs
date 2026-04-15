using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatView : MonoBehaviour {
    public Stat owner;
    [SerializeField]
    private Slider bar;
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetStat(Stat newStat) {
        owner = newStat;
        if(text) text.text = owner.currentValue + "|" + owner.maxValue;
        if (bar) {
            bar.maxValue = newStat.maxValue;
            bar.value = newStat.currentValue;
        }
        owner.OnChange += StatValueChanged;
    }

    private void StatValueChanged(int newValue) {
        if(text) text.text = owner.currentValue + "|" + owner.maxValue;
        StopAllCoroutines();
        if (bar) StartCoroutine(AnimateDamageTaken(newValue));
    }

    private void OnDestroy() {
        if (owner != null) {
            owner.OnChange -= StatValueChanged;
        }
    }

    public IEnumerator AnimateDamageTaken(int targetValue) {
        float duration = 0.25f;
        float elapsed = 0f;
        float startValue = bar.value;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            bar.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            yield return null;
        }

        bar.value = targetValue;
    }
}