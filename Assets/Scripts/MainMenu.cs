using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Doxel.Utility;

public class MainMenu : MonoBehaviour {

	private Button newGameButton;
	private Button playButton;
	private Button quitButton;
	private Animator animator;

	private void OnEnable () {
		playButton.onClick.AddListener (Play);
		quitButton.onClick.AddListener (Quit);
		newGameButton.onClick.AddListener (NewGame);
	}

	private void OnDisable() {
		playButton.onClick.RemoveListener (Play);
		quitButton.onClick.RemoveListener (Quit);
		newGameButton.onClick.RemoveListener (NewGame);
	}

	private void Awake () {
		newGameButton = GetComponentInChildren<Button> ();
		playButton = GetComponentsInChildren<Button> () [1];
		quitButton = GetComponentsInChildren<Button> () [2];
		animator = GetComponent<Animator> ();
	}

	private void Start () {
	}

	private void NewGame () {
		DataManager.Instance.ClearAll ();
		animator.SetTrigger ("Fly Out");
		StartCoroutine (Utility.DelayedInvokeRealTime (() => SceneController.Instance.LoadSceneFade ("Level Select"), 1));
	}

	private void Play () {
		animator.SetTrigger ("Fly Out");
		StartCoroutine (Utility.DelayedInvokeRealTime (() => SceneController.Instance.LoadSceneFade ("Level Select"), 1));
	}

	private void Quit () {
		EditorApplication.isPlaying = false;
		Application.Quit ();
	}

}