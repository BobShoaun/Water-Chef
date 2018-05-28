using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour {

	private Text text;
	private Button button;
	private Action OnClick = delegate {};

	public bool Interactable {
		set { 
			button.interactable = value;
		}
	}

	private void OnEnable () {
		button.onClick.AddListener (() => OnClick ());
		button.onClick.AddListener (() => AudioManager.Instance.PlaySoundEffect ("Plop"));
	}

	private void OnDisable () {
		button.onClick.RemoveAllListeners ();
	}

	private void Awake () {
		text = GetComponentInChildren<Text> ();
		button = GetComponentInChildren<Button> ();
	}

	private void Start () {
	}

	public void Show (Action onClick) {
		gameObject.SetActive (true);
		this.OnClick = onClick;
	}
		
	public void Show (string buttonText, Action onClick) {
		gameObject.SetActive (true);
		this.OnClick = onClick;
		text.text = buttonText;
	}

	public void Show (string buttonText, Action onClick, Vector2 globalPosition) {
		gameObject.SetActive (true);
		this.OnClick = onClick;
		text.text = buttonText;
		transform.position = globalPosition;
	}

	public void Hide () {
		gameObject.SetActive (false);
	}

}