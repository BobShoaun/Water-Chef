using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class PauseMenu : MonoBehaviour {

	public Image preventInteractionImage;
	private Button levelSelectButton;
	private Button continueButton;
	private Button mainMenuButton;
	private Slider masterVolumeSlider;
	private Slider musicVolumeSlider;
	private Slider soundEffectVolumeSlider;
	private Vector2 visiblePosition;
	private Vector2 invisiblePosition;
	public ConfirmationPrompt confirmationPrompt;
	private Animator animator;
	private bool isVisible;

	public bool IsVisible {
		get { return isVisible; }
		private set { 
			transform.localPosition = (isVisible = value) ? visiblePosition : invisiblePosition;
		}
	}

	private void OnEnable () {
		continueButton.onClick.AddListener (Continue);
		mainMenuButton.onClick.AddListener (() => confirmationPrompt.Show ("Are You Sure?", "Doing so would result in loosing all your level progress", MainMenu));
		levelSelectButton.onClick.AddListener (() => confirmationPrompt.Show ("Are You Sure?", "Doing so would result in loosing all your level progress", LevelSelect));
		masterVolumeSlider.onValueChanged.AddListener (value => AudioManager.Instance.MasterVolume = value);
		musicVolumeSlider.onValueChanged.AddListener (value => AudioManager.Instance.MusicVolume = value);
		soundEffectVolumeSlider.onValueChanged.AddListener (value => AudioManager.Instance.SoundEffectVolume = value);
	}

	private void OnDisable () {
		continueButton.onClick.RemoveAllListeners ();
		mainMenuButton.onClick.RemoveAllListeners ();
		levelSelectButton.onClick.RemoveAllListeners ();
		masterVolumeSlider.onValueChanged.RemoveAllListeners ();
		musicVolumeSlider.onValueChanged.RemoveAllListeners ();
		soundEffectVolumeSlider.onValueChanged.RemoveAllListeners ();
	}

	private void Awake () {
		levelSelectButton = GetComponentInChildren<Button> ();
		continueButton = GetComponentsInChildren<Button> () [1];
		mainMenuButton = GetComponentsInChildren<Button> () [2];
		masterVolumeSlider = GetComponentInChildren<Slider> ();
		musicVolumeSlider = GetComponentsInChildren<Slider> () [1];
		soundEffectVolumeSlider = GetComponentsInChildren<Slider> () [2];
		//confirmationPrompt = FindObjectOfType<ConfirmationPrompt> ();
		invisiblePosition = Vector2.up * 1000;
		visiblePosition = Vector2.zero;
		//IsVisible = false;
		animator = GetComponent<Animator> ();
	}

	private void Start () {
		masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
		musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
		soundEffectVolumeSlider.value = AudioManager.Instance.SoundEffectVolume;
	}

	public void Show () {
		StopAllCoroutines ();
		gameObject.SetActive (true);
		preventInteractionImage.gameObject.SetActive (true);
		StartCoroutine (Utility.Fade (result => preventInteractionImage.color = result, 0.5f, Color.clear, new Color (0, 0, 0, 0.5f)));
	}

	public void Hide () {
		animator.SetTrigger ("Close");
		StartCoroutine (Utility.DelayedInvokeRealTime (() => gameObject.SetActive (false), 1));
		StartCoroutine (Utility.Fade (result => preventInteractionImage.color = result, 0.5f, new Color (0, 0, 0, 0.5f), Color.clear, () => preventInteractionImage.gameObject.SetActive (false)));
	}

	private void Continue () {
		//AudioManager.Instance.PlaySoundEffect ("Biu", pitch: 1.2f);
		//Hide ();
		GameManager.Instance.Unpause ();
	}

	private void MainMenu () {
		//AudioManager.Instance.PlayOneShotSoundEffect ("Biu");
		SceneController.Instance.LoadSceneFade ("Main Menu");
		//Hide ();
		GameManager.Instance.Unpause ();
	}

	private void LevelSelect () {
		//AudioManager.Instance.PlayOneShotSoundEffect ("Biu");
		SceneController.Instance.LoadSceneFade ("Level Select");
		//Hide ();
		GameManager.Instance.Unpause ();
	}

}