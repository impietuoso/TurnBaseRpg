using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Drafts {
	/// <summary>optional serializable fields for views.</summary>
	public static class ExtensionOptionalFields {

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetText(this Text text, string value) { if(text) text.text = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetText(this Text text, object value) { if(text) text.text = value.ToString(); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetText(this TMP_Text text, string value) { if(text) text.text = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetText(this TMP_Text text, object value) { if(text) text.text = value.ToString(); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetColor(this Graphic graphic, Color value) { if(graphic) graphic.color = value; }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetText(this TMP_InputField input, string value) { if(input) input.SetTextWithoutNotify(value); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySubscribe(this TMP_InputField input, UnityAction<string> value) { if(input) input.onValueChanged.AddListener(value); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TryUnsubscribe(this TMP_InputField input, UnityAction<string> value) { if(input) input.onValueChanged.RemoveListener(value); }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetValue(this Slider slider, float value) { if(slider) slider.value = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetInteractable(this Selectable selectable, bool value) { if(selectable) selectable.interactable = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetSprite(this Image image, Sprite value) { if(image) { image.sprite = value; image.enabled = value; } }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetSprite(this SpriteRenderer rend, Sprite value) { if(rend) rend.sprite = value; }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TryEnable(this Behaviour behaviour, bool enabled) { if(behaviour) behaviour.enabled = enabled; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetActive(this GameObject go, bool active) { if(go) go.SetActive(active); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetActive(this Component c, bool active) { if(c) c.gameObject.SetActive(active); }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetAlpha(this CanvasGroup c, float value) { if(c) c.alpha = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetInteractable(this CanvasGroup c, bool value) { if(c) c.interactable = value; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetBlocksRaycasts(this CanvasGroup c, bool value) { if(c) c.blocksRaycasts = value; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TryPlay(this Animator anim, string stateName, int layer = 0) { if(anim && anim.isActiveAndEnabled) anim.Play(stateName, layer); }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TrySetData(this DataView view, object data) { if(view) view.SetData(data); }
	}
}