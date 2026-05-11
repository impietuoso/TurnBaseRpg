using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : DataView<Character> {
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private RadialBarConfig healthBar;
    [SerializeField] private RadialBarConfig manaBar;
    [SerializeField] private RadialBarConfig shieldBar;
    [SerializeField] private RadialBarConfig speedBar;
    [SerializeField] private StatusEffectListView statusEffectView;
    public Color normalSpeedColor;
    public Color fastSpeedColor;
    public Color slowSpeedColor;

    protected override void Subscribe() {
        healthBar.Stat = Data.derivedStats.health;
        manaBar.Stat = Data.derivedStats.mana;
        shieldBar.Stat = Data.derivedStats.shield;
        speedBar.Stat = Data.derivedStats.speed;

        Data.derivedStats.health.OnChange += healthBar.Update;
        Data.derivedStats.mana.OnChange += manaBar.Update;
        Data.derivedStats.shield.OnChange += shieldBar.Update;
        Data.derivedStats.speed.OnChange += speedBar.Update;

        Data.derivedStats.health.OnChange += ChangePortraitAlpha;
        Data.derivedStats.speed.OnChange += ChangeSpeedColor;

        if (statusEffectView) statusEffectView.SetData(Data.StatusEffectList);
        ChangeSpeedColor(Data.derivedStats.speed.currentValue);
        gameObject.SetActive(true);
    }

    protected override void Unsubscribe() {
        Data.derivedStats.health.OnChange -= healthBar.Update;
        Data.derivedStats.mana.OnChange -= manaBar.Update;
        Data.derivedStats.shield.OnChange -= shieldBar.Update;
        Data.derivedStats.speed.OnChange -= speedBar.Update;

        Data.derivedStats.health.OnChange -= ChangePortraitAlpha;
        Data.derivedStats.speed.OnChange -= ChangeSpeedColor;
        
        if (statusEffectView) statusEffectView.SetData(null);
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
    public class RadialBarConfig {
        public Image image;
        public float max;
        public Stat Stat { get; set; }
        public void Update(int _) => image.fillAmount = Stat.Normalized * max;
    }
}