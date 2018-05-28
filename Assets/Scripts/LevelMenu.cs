using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class LevelMenu : MonoBehaviour {

	public GameObject timesUp;
	public GameObject goalUIPrefab;
	public Transform goalPanel;

	public Image preventInteractionImage;

	public GameObject countdown;
	private Text headerText;
	public ProgressBar levelTimer;
	private ButtonUI dayButton;
	private StarGroup starGroup;
	private Animator animator;
	
	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		StopAllCoroutines ();
	}
	
	private void Awake () {
		animator = GetComponent<Animator> ();
		headerText = GetComponentInChildren<Text> ();
		dayButton = GetComponentInChildren<ButtonUI> ();
		starGroup = GetComponentInChildren<StarGroup> ();
	}

	private void Start () {
		
	}

	public void DisplayLevel (Level level) {
		Show ();
		dayButton.Show ("Start Day", () => GameManager.Instance.StartCoroutine (GameManager.Instance.StartLevel ()));
		headerText.text = level.Title;
		for (var i = 0; i < level.goals.Length; i++) {
			GoalUI goalUI = Instantiate (goalUIPrefab, goalPanel).GetComponent<GoalUI> ();
			goalUI.transform.localScale = Vector2.one;
			goalUI.Display (level.goals [i]);
		}
	}

	public void DisplaySuccess (Level level) {
		Show ();
		headerText.text = level.Title + " Completed";
		starGroup.ActivatedStars = level.starAmount;
		for (var i = 0; i < level.goals.Length; i++)
			goalPanel.GetChild (i).GetComponent<GoalUI> ().Display (level.goals [i]);
		dayButton.Show ("Start Next Day", () => GameManager.Instance.NextLevel ());
		AudioManager.Instance.PlaySoundEffect ("Victory");
		AudioManager.Instance.PlaySoundEffect ("Yay");
	}

	public void DisplayFail (Level level) {
		Show ();
		headerText.text = level.Title + " Failed";
		starGroup.ActivatedStars = level.starAmount;
		for (var i = 0; i < level.goals.Length; i++)
			goalPanel.GetChild (i).GetComponent<GoalUI> ().Display (level.goals [i]);
		dayButton.Show ("Retry Day", () => GameManager.Instance.StartCoroutine (GameManager.Instance.StartLevel ()));
		AudioManager.Instance.PlaySoundEffect ("Fail");
		AudioManager.Instance.PlaySoundEffect ("Crowd Gasp");
	}

	private void Show () {
		gameObject.SetActive (true);
		preventInteractionImage.gameObject.SetActive (true);
		StartCoroutine (Utility.Fade (result => preventInteractionImage.color = result, 0.5f, Color.clear, new Color (0, 0, 0, 0.5f)));
	}

	public void Hide () {
		StartCoroutine (Utility.Fade (result => preventInteractionImage.color = result, 0.5f, new Color (0, 0, 0, 0.5f), Color.clear, () => preventInteractionImage.gameObject.SetActive (false)));
		animator.SetTrigger ("Close");
		StartCoroutine (Utility.DelayedInvokeRealTime (() => gameObject.SetActive (false), 0.7f));
	}

}