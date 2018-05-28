using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrecisionBar : MonoBehaviour {

	public event Action Hit = delegate {};
	public event Action Miss = delegate {};

	public float top = -42;
	public float bottom = -540;
	private float height;
	private Vector2 initialMeterPosition;

	private Vector2 visiblePosition;
	private Vector2 invisiblePosition;
	private RectTransform meter;
	private float timer;
	public float Speed { get; set; }
	private bool moveMeter;
	[HideInInspector]
	public float precisionOffset;
	private bool executed;


	public bool IsVisible {
		set {
			transform.localPosition = value ? visiblePosition : invisiblePosition;
		}
	}

	void Awake () {
		invisiblePosition = new Vector2 (0, 10000);
		visiblePosition = transform.localPosition;
		meter = GetComponentsInChildren<RectTransform> () [1];
		initialMeterPosition = meter.anchoredPosition;
	}

	void Start () {
		height = top - bottom;
		IsVisible = false;
		Reset ();
	}

		
	void Update () {
		if (!moveMeter)
			return;
		timer += Time.deltaTime;
		meter.anchoredPosition = new Vector2 (initialMeterPosition.x, height / 2 * Mathf.Sin (timer * Speed) + initialMeterPosition.y);
	}

	public void Execute () {
		(meter.anchoredPosition.y > -precisionOffset * height + initialMeterPosition.y && meter.anchoredPosition.y < precisionOffset * height + initialMeterPosition.y ? Hit : Miss)();
	}

	public void Start (float speed) {
		Speed = speed;
		moveMeter = true;
	}

	public void Stop () {
		moveMeter = false;
	}

	public void Reset () {
		meter.anchoredPosition = initialMeterPosition;
		moveMeter = false;
		Speed = 0;
		timer = 0;
	}

}