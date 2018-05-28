using System;
using System.Collections;
using UnityEngine;

public class CountDown : MonoBehaviour {

	private Animator animator;
	
	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	private void Awake () {
		animator = GetComponent<Animator> ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	public void Ready () {
		AudioManager.Instance.PlaySoundEffect ("Countdown Start", volumeFactor: 0.5f, pitch: 1f);
		//AudioManager.Instance.PlaySoundEffect ("Ready", volumeFactor: 0.5f, pitch: 1.1f);
	}

	public void Set () {
		AudioManager.Instance.PlaySoundEffect ("Countdown Start", volumeFactor: 0.5f, pitch: 1f);
		//AudioManager.Instance.PlaySoundEffect ("Set", volumeFactor: 0.5f, pitch: 1.1f);
	}

	public void Go () {
		AudioManager.Instance.PlaySoundEffect ("Countdown Final", volumeFactor: 0.5f, pitch: 1f);
		//AudioManager.Instance.PlaySoundEffect ("Go", volumeFactor: 0.5f, pitch: 1.1f);
	}

}