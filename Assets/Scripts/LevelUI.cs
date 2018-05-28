using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class LevelUI : MonoBehaviour {

	private Button selectButton;
	private Text signText;
	public Level level;
	public GameObject levelLockedMessage;
	private StarGroup starGroup;
	private Image buttonImage;

	[SerializeField]
	private Sprite locked;
	[SerializeField]
	private Sprite unlocked;
	[SerializeField]
	private Sprite played;
	
	private void OnEnable () {
		selectButton.onClick.AddListener (Select);
	}
	
	private void OnDisable () {
		selectButton.onClick.RemoveAllListeners ();
		StopAllCoroutines ();
	}
	
	private void Awake () {
		starGroup = GetComponentInChildren<StarGroup> ();
		selectButton = GetComponentInChildren<Button> ();
		buttonImage = selectButton.GetComponent<Image> ();
		signText = GetComponentInChildren<Text> ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	public void Display (Level level) {
		this.level = level;
		signText.text = level.Title;
		starGroup.ActivatedStars = level.starAmount;
		if (level.unlocked)
			buttonImage.sprite = unlocked;
		else if (level.played)
			buttonImage.sprite = played;
		else
			buttonImage.sprite = locked;
	}

	private void Select () {
		if (level.unlocked) { 
			GameManager.Instance.currentLevel = level;
			SceneController.Instance.LoadSceneFade ("Level " + (level.Id + 1));
		} else {
			StopAllCoroutines ();
			levelLockedMessage.SetActive (true);
			StartCoroutine (Utility.DelayedInvokeRealTime (() => levelLockedMessage.SetActive (false), 2));
		}
	}

}