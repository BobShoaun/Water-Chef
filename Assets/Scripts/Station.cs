using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class Station : MonoBehaviour, IIdentifiable {

	[SerializeField]
	private int id;
	[SerializeField]
	private string title;
	[SerializeField]
	private Transform useButtonTransform;
	[SerializeField]
	private ButtonUI useButton;
	[SerializeField]
	private ButtonUI backButton;
	private Camera stationCamera;
	private Camera mainCamera;
	private CanvasGroup canvasGroup;
	private Canvas screenCameraCanvas;
	public bool entered;
	private bool inTriggerCollider;
	public bool canEnterAndExit;

	public int Id {
		get { return id; }
		set { id = value; }
	}

	public string Title {
		get { return title; }
	}

	private void Awake () {
		screenCameraCanvas = GameObject.FindGameObjectWithTag ("Screen Camera Canvas").GetComponent<Canvas> ();
		stationCamera = GetComponentInChildren<Camera> ();
		mainCamera = Camera.main;
		canvasGroup = GetComponentInChildren<CanvasGroup> ();
	}

	private void Start () {
		canvasGroup.blocksRaycasts = false;
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space) && canEnterAndExit) {
			if (inTriggerCollider && !entered)
				Enter ();
			else if (entered)
				Exit ();
		}
	}

	private void OnTriggerEnter2D () {
		useButton.Show (title, Enter, useButtonTransform.position);
		inTriggerCollider = true;
	}

	private void OnTriggerExit2D () {
		useButton.Hide ();
		inTriggerCollider = false;
	}

	private void Enter () {
		entered = true;
		EventManager<bool>.Instance.Broadcast (Event.InStation, true);
		stationCamera.enabled = true;
		mainCamera.enabled = false;
		useButton.Hide ();
		backButton.Show ("Go Back", Exit);
		canvasGroup.blocksRaycasts = true;
		screenCameraCanvas.worldCamera = stationCamera;
	}

	public void Exit () {
		entered = false;
		EventManager<bool>.Instance.Broadcast (Event.InStation, false);
		stationCamera.enabled = false;
		mainCamera.enabled = true;
		useButton.Show (title, Enter, useButtonTransform.position);
		backButton.Hide ();
		canvasGroup.blocksRaycasts = false;
		screenCameraCanvas.worldCamera = mainCamera;
	}

}