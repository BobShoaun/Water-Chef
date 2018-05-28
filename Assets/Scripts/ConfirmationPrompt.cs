using System;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class ConfirmationPrompt : MonoBehaviour {

	private Text headerText;
	private Text contentText;
	private Button yesButton;
	private Button noButton;
	private Action affirmative;
	private Animator animator;

	public Image preventInteractionImage;

	private void OnEnable () {
		yesButton.onClick.AddListener (() => affirmative ());
		noButton.onClick.AddListener (Hide);
	}
	
	private void OnDisable () {
		yesButton.onClick.RemoveAllListeners ();
		noButton.onClick.RemoveAllListeners ();
	}
	
	private void Awake () {
		headerText = GetComponentInChildren<Text> ();
		contentText = GetComponentsInChildren<Text> () [1];
		yesButton = GetComponentInChildren<Button> ();
		noButton = GetComponentsInChildren<Button> () [1];
		animator = GetComponent<Animator> ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	public void Show (string header, string content, Action affirmative) {
		gameObject.SetActive (true);
		this.affirmative += affirmative += Hide;
		headerText.text = header;
		contentText.text = content;
		preventInteractionImage.gameObject.SetActive (true);
		StartCoroutine (Utility.Fade (result => preventInteractionImage.color = result, 0.5f, Color.clear, new Color (0, 0, 0, 0.5f)));
	}

	public void Hide () {
		animator.SetTrigger ("Close");
		StartCoroutine (Utility.DelayedInvokeRealTime (() => gameObject.SetActive (false), 1));
		StartCoroutine (Utility.Fade (result => preventInteractionImage.color = result, 0.5f, new Color (0, 0, 0, 0.5f), Color.clear, () => preventInteractionImage.gameObject.SetActive (false)));
	}

}