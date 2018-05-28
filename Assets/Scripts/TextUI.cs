using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour {

	private int initialFontSize;
	private Text text;
	private Color textColor;
	[HideInInspector] 
	public Color initialColor;
	private Vector2 visiblePosition;
	private Vector2 invisiblePosition = new Vector2 (0, 10000);
	private bool isVisible;

	public bool IsVisible {
		get { return isVisible; }
		set {
			transform.localPosition = (isVisible = value) ? visiblePosition : invisiblePosition;
		}
	}

	public Color TextColor {
		set { 
			text.color = textColor = value;
		}
	}
		
	private void Awake () {
		text = GetComponentInChildren<Text> ();
		invisiblePosition = new Vector2 (0, 10000);
		visiblePosition = transform.localPosition;
		initialColor = text.color;
		initialFontSize = text.fontSize;
	}

	private void Start () {
		IsVisible = false;
	}

	public void Show (string message) {
		text.text = message;
		IsVisible = true;
	}

	public void Show (string message, float seconds) {
		StopAllCoroutines ();
		text.text = message;
		IsVisible = true;
		StartCoroutine (HideCallBack (seconds));
	}

	public void Show (string message, int fontSize, Color textColor) {
		text.fontSize = fontSize;
		text.color = textColor;
		text.text = message;
		IsVisible = true;
	}

	private IEnumerator HideCallBack (float seconds) {
		yield return new WaitForSeconds (seconds);
		Hide ();
	}

	public void Hide () {
		text.fontSize = initialFontSize;
		text.color = initialColor;
		text.text = string.Empty;
		IsVisible = false;
	}

}