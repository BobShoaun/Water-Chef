using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public event Action Finish = delegate {};
	public event Action Full = delegate {};

	public float speed;
	private Image meter;
	public float finishAmount;

	private bool canMove;
	private bool hasRan;

	public float FillAmount {
		get { 
			return meter.fillAmount;
		}
	}

	private void Awake () {
		meter = GetComponentsInChildren<Image> () [1];
	}

	private void Start () {
		Reset ();
	}
		
	private void Update () {
		if (!canMove)
			return;
		meter.fillAmount += speed * Time.deltaTime;
		if (meter.fillAmount > finishAmount && !hasRan) {
			Finish ();
			hasRan = true;
		}
		if (meter.fillAmount >= 1) {
			Full ();
			Stop ();
		}
	}

	public void Activate (float speed) {
		gameObject.SetActive (true);
		Start (speed);
	}

	public void Deactivate () {
		Stop ();
		Reset ();
		gameObject.SetActive (false);
	}

	public void Start (float speed) {
		this.speed = speed;
		canMove = true;
	}

	public void Stop () {
		speed = 0;
		canMove = false;
	}

	public void Reset () {
		meter.fillAmount = 0;
		hasRan = false;
	}

}