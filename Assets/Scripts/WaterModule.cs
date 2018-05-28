using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterModule : Module {

	private ProgressBar progressBar;
	private int audioPlayId = 0;

	protected override void OnDisable () {
		base.OnDisable ();
		progressBar.Full -= PotFull;
	}

	protected override void Awake () {
		base.Awake ();
		progressBar = GetComponentInChildren<ProgressBar> ();
	}

	private void Start () {
		progressBar.Full += PotFull;
	}

	protected override void Activate () {
		progressBar.Activate (0.5f);
		audioPlayId = AudioManager.Instance.PlaySoundEffect (audioPlayId, "Water Pour");
	}

	protected override void Deactivate () {
		progressBar.Deactivate ();
		AudioManager.Instance.StopSoundEffect (audioPlayId);
	}

	private void PotFull() {
		ReplaceItem (stationSlot, ItemDatabase.Instance ["Pot Of Water"]);
		AudioManager.Instance.StopSoundEffect (audioPlayId);
	}

}