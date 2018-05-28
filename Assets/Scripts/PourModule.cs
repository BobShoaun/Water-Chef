using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class PourModule : Module {
	
	[SerializeField]
	private int triesCount = 3;
	private static int triesLeft;
	private Item pot;
	private Item cupOfWater;
	private StationSlot[] stationSlots;
	private PrecisionBar precisionBar;
	private TextUI textPrompt;
	private ButtonUI pourButton;
	private Animator animator;
	private int pourPlayId;

	protected override void OnEnable () {
		precisionBar.Miss += Spill;
		precisionBar.Hit += Pour;
		foreach (var stationSlot in stationSlots)
			stationSlot.ItemChanged += CheckSlots;
	}

	protected override void OnDisable () {
		precisionBar.Miss -= Spill;
		precisionBar.Hit -= Pour;
		foreach (var stationSlot in stationSlots)
			stationSlot.ItemChanged -= CheckSlots;
		Deactivate ();
	}

	protected override void Awake () {
		pourButton = GetComponentInChildren<ButtonUI> ();
		precisionBar = GetComponentInChildren<PrecisionBar> ();
		textPrompt = GetComponentInChildren<TextUI> ();
		stationSlots = GetComponentsInChildren<StationSlot> ();
		animator = GetComponent<Animator> ();
	}
		
	private void Start () {
		cupOfWater = ItemDatabase.Instance ["Cup Of Water"];
		pot = ItemDatabase.Instance ["Pot"];
		precisionBar.precisionOffset = 0.05f;
		triesLeft = triesCount;
	}
		
	protected override void CheckSlots () {
		foreach (var stationSlot in stationSlots) {
			if (stationSlot.CheckItem () == ItemMode.Activate)
				continue;
			if (stationSlot.CheckItem () == ItemMode.Deactivate || !expectedItemChange)
				Deactivate ();
			return;
		}
		Activate ();
	}
		
	protected override void Activate() {
		precisionBar.IsVisible = true;
		precisionBar.Start (3);
		pourButton.Hide ();
		pourButton.Show ("Pour!", precisionBar.Execute);
		textPrompt.Show ("Smack spacebar when the meter is in the middle!", 3);
	}

	protected override void Deactivate () {
		pourButton.Hide ();
		textPrompt.Hide ();
		precisionBar.IsVisible = false;
		precisionBar.Reset ();
	}

	private void Spill() {
		pourButton.Hide ();
		precisionBar.Stop ();
		triesLeft--;
		if (triesLeft <= 0)
			Punish ();
		else
			StartCoroutine (TryAgain ());
	}

	private void Pour () {
		animator.SetTrigger ("Pouring");
		animator.runtimeAnimatorController.animationClips [0].events [0].functionName = "PourComplete";
		pourButton.Hide ();
		precisionBar.Stop ();
		textPrompt.Show ("Nice!", 2);
		triesLeft = triesCount;
		foreach (var stationSlot in stationSlots) {
			stationSlot.interactable = false;
			ReplaceItem (stationSlot, ItemDatabase.Instance.Nothing);
		}
		AudioManager.Instance.PlaySoundEffect ("TrumpetTaDa");
		pourPlayId = AudioManager.Instance.PlaySoundEffect ("PourGlass");
	}

	private void PourComplete () {
		AudioManager.Instance.PlaySoundEffect ("Ting");
		for (var i = 0; i < stationSlots.Length - 1; i++)
			ReplaceItem (stationSlots [i], cupOfWater); 
		ReplaceItem (stationSlots [stationSlots.Length - 1], pot);
		foreach (var stationSlot in stationSlots)
			stationSlot.interactable = true;
		AudioManager.Instance.StopSoundEffect (pourPlayId);
	}

	private IEnumerator TryAgain() {
		textPrompt.Show ("You're a failure. Tries Left: " + triesLeft, 3);
		AudioManager.Instance.PlaySoundEffect ("Wrong");
		yield return new WaitForSeconds (0.5f);
		Image itemImage = stationSlots [stationSlots.Length - 1].Item.GetComponent<Image> ();
		yield return StartCoroutine (Utility.Fade (result => itemImage.color = result, 0.2f, Color.white, Color.red));
		yield return StartCoroutine (Utility.Fade (result => itemImage.color = result, 0.2f, Color.red, Color.white));
		precisionBar.Start (3);
		pourButton.Show ("Pour!", precisionBar.Execute);
	}

	private void Punish() {
		triesLeft = triesCount;
		precisionBar.Reset ();
		precisionBar.IsVisible = false;
		textPrompt.Show ("Heres your poop", 3);
		//foreach (var stationSlot in stationSlots)
		//	stationSlot.Item = poop;
	}

}