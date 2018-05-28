using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CupController : MonoBehaviour {

	public event Action IceCountChanged = delegate {};
	[SerializeField]
	private float speed;
	private Rigidbody2D rigidBody2D;
	private int iceCount;
	[HideInInspector]
	public bool canMove;
	[HideInInspector]
	public float maxBoundary;
	[HideInInspector]
	public float minBoundary;
	private float left;
	private float right;

	public int IceCount{
		get { return iceCount; }
		set { 
			iceCount = value;
			IceCountChanged ();
		}
	}

	private void Awake () {
		rigidBody2D = GetComponent<Rigidbody2D> ();
		canMove = false;
	}

	private void Update () {
		float input = Input.GetAxis ("Horizontal");
		left = TryReadLeftInput (input);
		right = TryReadRightInput (input);
	}

	private void FixedUpdate() {
		if (right > 0) {
			rigidBody2D.velocity = Vector2.right * right * speed;
			return;
		}
		if (left > 0) {
			rigidBody2D.velocity = Vector2.left * left * speed;
			return;
		}
		rigidBody2D.velocity = Vector2.zero;
	}

	private float TryReadLeftInput (float input) {
		return canMove ? transform.localPosition.x > minBoundary ? input < 0 ? Mathf.Abs (input) : 0 : 0 : 0;
	}

	private float TryReadRightInput (float input) {
		return canMove ? transform.localPosition.x < maxBoundary ? input > 0 ? input : 0 : 0 : 0;
	}
		
	private void OnTriggerEnter2D (Collider2D other) {
		IceCount++;
		AudioManager.Instance.PlaySoundEffect ("IceDrop1", "IceDrop2", "IceDrop3");
	}

	private void OnTriggerExit2D () {
		IceCount--;
	}

}