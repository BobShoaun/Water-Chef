using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour {

	private LevelUI[] levelUIs;
	public Button backToMainMenuButton;
	
	private void OnEnable () {
		backToMainMenuButton.onClick.AddListener (() => SceneController.Instance.LoadSceneFade ("Main Menu"));
	}
	
	private void OnDisable () {
		backToMainMenuButton.onClick.RemoveAllListeners ();
	}
	
	private void Awake () {
		levelUIs = GetComponentsInChildren<LevelUI> ();
	}

	private void Start () {
		for (var i = 0; i < levelUIs.Length; i++)
			levelUIs [i].Display (GameManager.Instance.levels [i]);
	}
		
}

