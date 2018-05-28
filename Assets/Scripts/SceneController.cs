using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Doxel.Utility;
using Doxel.Utility.ExtensionMethods;

public class SceneController : SingletonMonoBehaviour<SceneController> {

	/// <summary> Called once in the lifetime of the application after all OnEnables. </summary>
	public event Action ApplicationStart = delegate {};
	/// <summary> Called once in the lifetime of the application before all OnDisables. </summary>
	public event Action ApplicationQuit = delegate {};
	/// <summary> Called before a scene unloads before all OnDisables. </summary>
	public event Action SceneUnload = delegate {};
	/// <summary> Called after a scene is loaded after all OnEnables. </summary>
	public event Action SceneLoaded = delegate {};

	[SerializeField]
	private AnimationCurve fadeCurve;
	private Image fadeImage;
	private bool fade;

	public Scene CurrentScene {
		get {
			return SceneManager.GetActiveScene ();
		}
	}

	private void OnEnable () {
		SceneManager.sceneLoaded += (scene, loadSceneMode) => OnSceneLoaded ();
	}

	private void OnDisable () {
		SceneManager.sceneLoaded -= (scene, loadSceneMode) => OnSceneLoaded ();
	}

	private void Start () {
		ApplicationStart ();
		fadeImage = GetComponentInChildren<Image> ();
	}

	private void OnApplicationQuit () {
		ApplicationQuit ();
	}

	private void OnSceneLoaded () {
		SceneLoaded ();
		if (fade) {
			fade = false;
			StartCoroutine (Utility.Fade (result => fadeImage.color = result, 0.2f, Color.black, Color.clear, fadeCurve));
		}
	}

	public void LoadScene (int sceneBuildIndex) {
		SceneUnload ();
		SceneManager.LoadScene (sceneBuildIndex);
	}

	public void LoadScene (string sceneName) {
		SceneUnload ();
		SceneManager.LoadScene (sceneName);
	}

	public void LoadSceneAsync (int sceneBuildIndex) {
		SceneUnload ();
		SceneManager.LoadSceneAsync (sceneBuildIndex);
	}

	public void LoadSceneFade (int sceneBuildIndex) {
		fade = true;
		StartCoroutine (Utility.Fade (result => fadeImage.color = result, 0.2f, Color.clear, Color.black, fadeCurve, () => LoadScene (sceneBuildIndex)));
	}

	public void LoadSceneFade (string sceneName) {
		fade = true;
		StartCoroutine (Utility.Fade (result => fadeImage.color = result, 0.2f, Color.clear, Color.black, fadeCurve, () => LoadScene (sceneName)));
	}

}