using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Doxel.Utility;

public class Radio : MonoBehaviour, IPointerDownHandler {

	[SerializeField]
	private float playTime;
	[SerializeField]
	private float patienceBoostPercent = 0.2f;
	private float patienceBoost;
	private ParticleSystem musicNotesParticles;
	private Animator animator;
	private bool isPlaying = false;

	private void Awake () {
		musicNotesParticles = GetComponentInChildren<ParticleSystem> ();
		patienceBoost = CustomerManager.Instance.CustomerPatience * patienceBoostPercent;
		animator = GetComponent<Animator> ();
	}

	public void OnPointerDown (PointerEventData eventData) {
		Play ();
	}

	private void Play () {
		musicNotesParticles.Play ();
		CustomerManager.Instance.CustomerPatience += patienceBoost;
		StartCoroutine (Utility.DelayedInvokeRealTime (Stop, playTime));
		animator.SetBool ("IsPlaying", true);
	}

	private void Stop () {
		CustomerManager.Instance.CustomerPatience -= patienceBoost;
		musicNotesParticles.Stop ();
		animator.SetBool ("IsPlaying", false);
	}

}