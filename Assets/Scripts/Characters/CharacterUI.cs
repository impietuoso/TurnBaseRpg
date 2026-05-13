using System;
using Drafts;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : DataView<Character> {
    [SerializeField] private Image characterSprite;
    [SerializeField] private FillStatView healthBar;
    [SerializeField] private FillStatView manaBar;
    [SerializeField] private FillStatView shieldBar;
    [SerializeField] private FillStatView actionBar;
    [SerializeField] private StatusEffectListView statusEffectView;
    public Color normalSpeedColor;
    public Color fastSpeedColor;
    public Color slowSpeedColor;

    private void Start() {
        healthBar.Max = healthBar.image.fillAmount;
        manaBar.Max = manaBar.image.fillAmount;
        shieldBar.Max = shieldBar.image.fillAmount;
        actionBar.Max = actionBar.image.fillAmount;
    }

    protected override void Subscribe() {
        healthBar.Stat = Data.Health;
        manaBar.Stat = Data.Mana;
        shieldBar.Stat = Data.Shield;

        Data.Health.OnChanged += healthBar.Update;
        Data.Mana.OnChanged += manaBar.Update;
        Data.Shield.OnChanged += shieldBar.Update;

        Data.Health.OnChanged += ChangePortraitAlpha;
        Data.Stats.Speed.OnChanged += ChangedSpeedColor;

        if (statusEffectView) statusEffectView.SetData(Data.StatusEffectList);
        characterSprite.TrySetSprite(Data.Member.uiSprite);

        ChangedSpeedColor(Data.Stats.Speed.Total);
    }

    protected override void Unsubscribe() {
        Data.Health.OnChanged -= healthBar.Update;
        Data.Mana.OnChanged -= manaBar.Update;
        Data.Shield.OnChanged -= shieldBar.Update;

        Data.Health.OnChanged -= ChangePortraitAlpha;
        Data.Stats.Speed.OnChanged -= ChangedSpeedColor;

        if (statusEffectView) statusEffectView.SetData(null);
    }

    private void Update() {
        if (Data.InAction) actionBar.SetPercent(1);
        else actionBar.SetPercent(Data.NextAction?.Progress ?? 0);
    }

    private void ChangePortraitAlpha(ResourceStat stat, int _) {
        characterSprite.color = stat.Current == 0 ? new Color(1, 1, 1, 0.5f) : Color.white;
    }

    private void ChangedSpeedColor(int value) {
        var @base = Data.Stats.Speed.Base;
        var sliderColor = value == @base ? normalSpeedColor : value > @base ? fastSpeedColor : slowSpeedColor;
        actionBar.image.color = sliderColor;
    }

    [Serializable]
    public class FillStatView {
        public Image image;
        public float Max { get; set; }
        public ResourceStat Stat { get; set; }
        public void Update(ResourceStat stat, int delta) => image.fillAmount = Stat.Normalized * Max;
        public void SetPercent(float p) => image.fillAmount = p * Max;
    }
}