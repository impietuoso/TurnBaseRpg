using TMPro;
using UnityEngine;

namespace Drafts
{
    public class FormatedText : DataView<object>
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string format = "{0}";
        private string _defaultText;

        private void Awake() => _defaultText = text.text;
        private void Set(object value) => text.text = string.Format(format, value);

        public void SetFormat(string f)
        {
            format = f;
            Set(Data);
        }

        public void SetValue(object value) => Set(value ?? _defaultText);
        public void SetValue(string value) => Set(value ?? _defaultText);
        public void SetValue(int value) => Set(value);
        public void SetValue(float value) => Set(value);

        protected override void Subscribe() => Set(Data);
        protected override void Unsubscribe() => Set(_defaultText);
    }

    public static class FormatedTextExtensions
    {
        public static void TrySetValue(this FormatedText view, object value)
        {
            if (view) view.SetValue(value);
        }

        public static void TrySetValue(this FormatedText view, string value)
        {
            if (view) view.SetValue(value);
        }

        public static void TrySetValue(this FormatedText view, int value)
        {
            if (view) view.SetValue(value);
        }

        public static void TrySetValue(this FormatedText view, float value)
        {
            if (view) view.SetValue(value);
        }
    }
}