using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceModule : Module {

	public GameObject icePrefab;
	[SerializeField]
	private int fullIceCount;
	[SerializeField]
	private float minBoundary;
	[SerializeField]
	private float maxBoundary;
	private Text scoreText;
	private CupController cupController;

	protected override void OnEnable () {
		base.OnEnable ();
		cupController.IceCountChanged += DisplayIceCount;
	}

	protected override void OnDisable () {
		base.OnDisable ();
		cupController.IceCountChanged -= DisplayIceCount;
	}

	protected override void Awake () {
		base.Awake ();
		scoreText = GetComponentInChildren<Text> ();
		cupController = GetComponentInChildren<CupController> ();
		cupController.minBoundary = minBoundary;
		cupController.maxBoundary = maxBoundary;
	}

	protected override void Activate () {
		InvokeRepeating ("SpawnIce", 1, 1);
		ReplaceItem (stationSlot, ItemDatabase.Instance.Nothing);
		stationSlot.interactable = false;
		scoreText.gameObject.SetActive (true);
		scoreText.text = "0/" + fullIceCount;
		cupController.canMove = true;
	}

	protected override void Deactivate () {
		print ("deactivate");
		cupController.IceCount = 0;
		scoreText.gameObject.SetActive (false);
		cupController.canMove = false;
	}

	private void DisplayIceCount () {
		scoreText.text = cupController.IceCount + "/" + fullIceCount;
		if (cupController.IceCount == fullIceCount) {
			Win ();
			scoreText.text = fullIceCount + "/" + fullIceCount;
		}
	}
		
	private void Win () {
		cupController.canMove = false;
		stationSlot.interactable = true;
		CancelInvoke ();
		ReplaceItem (stationSlot, ItemDatabase.Instance ["Cup Of Ice Water"]);
		foreach (var ice in GameObject.FindGameObjectsWithTag ("Ice"))
			Destroy (ice);
	}

	private void SpawnIce () {
		GameObject ice = Instantiate (icePrefab, transform);
		ice.transform.localPosition = new Vector2 (Random.Range (minBoundary, maxBoundary), 190);
		//ice.transform.localScale = new Vector2 (0.4f, 0.5f);
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Ice")
			Destroy (other.gameObject);
	}

}