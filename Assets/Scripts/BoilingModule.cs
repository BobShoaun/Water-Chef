using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class BoilingModule : Module {

	private Item potOfBoiledWater;
	private Button stoveDial;
	private ProgressBar progressBar;
	private int boilingSfxPlayId = 0;
	private int fireSfxPlayId;
	private bool doneBoiling;
	private ParticleSystem fireParticles;
	private Animator animator;

	protected override void OnEnable () {
		base.OnEnable ();
		progressBar.Full += FinishBoiling;

	}

	protected override void OnDisable() {
		base.OnDisable ();
		progressBar.Full -= FinishBoiling;
	}

	protected override void Awake() {
		base.Awake ();
		stoveDial = GetComponentInChildren<Button> ();
		animator = stoveDial.GetComponent<Animator> ();
		progressBar = GetComponentInChildren<ProgressBar> ();
		fireParticles = GetComponentInChildren<ParticleSystem> ();
	}

	private void Start () {
		potOfBoiledWater = ItemDatabase.Instance ["Pot Of Boiled Water"];
	}

	protected override void Activate(){
		stoveDial.interactable = true;
		stoveDial.onClick.AddListener (StartBoiling);
		doneBoiling = false;
	}

	protected override void Deactivate() {
		StopAllCoroutines ();
		stoveDial.onClick.RemoveAllListeners ();
		stoveDial.interactable = false;
		progressBar.gameObject.SetActive (false);
		progressBar.Reset ();
		AudioManager.Instance.StopSoundEffect (boilingSfxPlayId);
		DoneBoiling ();
	}
		
	public void StartBoiling () {
		stoveDial.onClick.RemoveAllListeners ();
		stoveDial.onClick.AddListener (StopBoiling);
		animator.SetBool ("Dial State", true);
		progressBar.Activate (0.3f);
		boilingSfxPlayId = AudioManager.Instance.PlaySoundEffect (boilingSfxPlayId,"Boiling", volumeFactor : 1.2f, loop : true);
	}

	public void StopBoiling () {
		stoveDial.onClick.RemoveAllListeners ();
		stoveDial.onClick.AddListener (StartBoiling);
		animator.SetBool ("Dial State", false);
		progressBar.Activate (-0.02f);
		AudioManager.Instance.PauseSoundEffect (boilingSfxPlayId);
	}

	public void FinishBoiling () {
		ReplaceItem (stationSlot, potOfBoiledWater);
		progressBar.Deactivate ();
		stoveDial.onClick.RemoveAllListeners ();
		stoveDial.onClick.AddListener (DoneBoiling);
		StartCoroutine (AnimateOverBoiledSequence ());
	}

	private void DoneBoiling () {
		stoveDial.onClick.RemoveAllListeners ();
		stoveDial.interactable = false;
		animator.SetBool ("Dial State", false);
		doneBoiling = true;
		fireParticles.Stop ();
		AudioManager.Instance.StopSoundEffect (boilingSfxPlayId);
		AudioManager.Instance.StopSoundEffect (fireSfxPlayId);
	}

	private IEnumerator AnimateOverBoiledSequence () {
		yield return new WaitForSeconds (3);
		var flashCount = 0;
		var itemImage = stationSlot.Item.GetComponent<Image> ();
		while (!doneBoiling) {
			yield return StartCoroutine (Utility.Fade (result => itemImage.color = result, 1, Color.white, Color.red));
			yield return StartCoroutine (Utility.Fade (result => itemImage.color = result, 1, Color.red, Color.white));
			flashCount++;
			if (flashCount == 3) {
				fireSfxPlayId = AudioManager.Instance.PlaySoundEffect ("Fire");
				fireParticles.Play ();
			}
			if (flashCount == 7)
				ReplaceItem (stationSlot, ItemDatabase.Instance ["Burnt Pot"]);
		}
	}

}