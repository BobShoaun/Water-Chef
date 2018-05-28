using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Module : MonoBehaviour {

	protected StationSlot stationSlot;
	protected bool expectedItemChange;

	protected virtual void OnEnable() {
		stationSlot.ItemChanged += CheckSlots;
	}

	protected virtual void OnDisable() {
		stationSlot.ItemChanged -= CheckSlots;
		Deactivate ();
	}

	protected virtual void Awake () {
		stationSlot = GetComponentInChildren<StationSlot> ();
	}

	protected virtual void CheckSlots () {
		if (stationSlot.CheckItem () == ItemMode.Activate)
			Activate ();
		else if (stationSlot.CheckItem () == ItemMode.Deactivate || !expectedItemChange)
			Deactivate ();
	}

	protected void ReplaceItem (StationSlot slot, Item itemToReplaceWith) {
		expectedItemChange = true;
		slot.Item = itemToReplaceWith;
		expectedItemChange = false;
	}

	protected abstract void Activate ();

	protected abstract void Deactivate ();

}