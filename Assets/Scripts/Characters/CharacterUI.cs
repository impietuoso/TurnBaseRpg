using System;
using Drafts;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : DataView<Character> {
    [SerializeField] private Image characterSprite;
    [SerializeField] private FillStatView healthBar;
    [SerializeField] private FillStatView manaBar;
    [SerializeField] private FillStatView shieldBar;
    [SerializeField] private FillStatView speedBar;
    [SerializeField] private StatusEffectListView statusEffectView;
    public Color normalSpeedColor;
    public Color fastSpeedColor;
    public Color slowSpeedColor;

    private void Start() {
        healthBar.Max = healthBar.image.fillAmount;
        manaBar.Max = manaBar.image.fillAmount;
        shieldBar.Max = shieldBar.image.fillAmount;
        speedBar.Max = speedBar.image.fillAmount;
    }

    protected override void Subscribe() {
        healthBar.Stat = Data.derivedStats.health;
        manaBar.Stat = Data.derivedStats.mana;
        shieldBar.Stat = Data.derivedStats.shield;

        Data.derivedStats.health.OnChange += healthBar.Update;
        Data.derivedStats.mana.OnChange += manaBar.Update;
        Data.derivedStats.shield.OnChange += shieldBar.Update;

        Data.derivedStats.health.OnChange += ChangePortraitAlpha;
        Data.derivedStats.speed.OnChange += ChangeSpeedColor;

        if (statusEffectView) statusEffectView.SetData(Data.StatusEffectList);
        characterSprite.TrySetSprite(Data.Member.uiSprite);

        ChangeSpeedColor(Data.derivedStats.speed.currentValue);
    }

    protected override void Unsubscribe() {
        Data.derivedStats.health.OnChange -= healthBar.Update;
        Data.derivedStats.mana.OnChange -= manaBar.Update;
        Data.derivedStats.shield.OnChange -= shieldBar.Update;

        Data.derivedStats.health.OnChange -= ChangePortraitAlpha;
        Data.derivedStats.speed.OnChange -= ChangeSpeedColor;

        if (statusEffectView) statusEffectView.SetData(null);
    }

    private void Update() {
        if (Data.InAction) speedBar.SetPercent(1);
        else speedBar.SetPercent(Data.NextAction?.Progress ?? 0);
    }

    private void ChangePortraitAlpha(int i) {
        characterSprite.color = i == 0 ? new Color(1, 1, 1, 0.5f) : Color.white;
    }

    private void ChangeSpeedColor(int value) {
        var baseValue = Data.derivedStats.speed.baseValue;
        var sliderColor = value == baseValue ? normalSpeedColor : value > baseValue ? fastSpeedColor : slowSpeedColor;
        speedBar.image.color = sliderColor;
    }

    [Serializable]
    public class FillStatView {
        public Image image;
        public float Max { get; set; }
        public Stat Stat { get; set; }
        public void Update(int _) => image.fillAmount = Stat.Normalized * Max;
        public void SetPercent(float p) => image.fillAmount = p * Max;
    }
}