using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class StationSlot : Slot {

	public event Action InvalidItemPlaced = delegate {};
	public event Action PointerDown = delegate {};
	public ValidItem[] ValidItems;
	private float lastClickTime;
	private float maxDoubleClickInterval = 0.2f;
	private Inventory inventory;

	protected override void Awake () {
		base.Awake ();
		inventory = FindObjectOfType<Inventory> ();
	}

	public ItemMode CheckItem () {
		if (ValidItems.Length == 0 && item.Id != 0)
			return ItemMode.Activate;
		foreach (ValidItem validItem in ValidItems) {
			if (validItem.item.Id != item.Id)
				continue;
			if (validItem.itemMode == ItemMode.Activate)
				return ItemMode.Activate;
			else if (validItem.itemMode == ItemMode.Deactivate)
				return ItemMode.Deactivate;
			else
				return ItemMode.Maintain;
		}
		return ItemMode.Deactivate;
	}

	public override void OnPointerDown (PointerEventData eventData) {
		if (!interactable)
			return;
		if (draggedItem == null) {
			PointerDown ();
			if (item.Id != 0)
				BeginDrag ();
		}
		else if (FilterItem (draggedItem)) {
			if (item.Id == 0)
				EndDrag ();
			else
				DragSwitch ();
		}
		if (Time.time - lastClickTime < maxDoubleClickInterval)
			OnPointerDoubleClick ();
		lastClickTime = Time.time;
	}

	private void OnPointerDoubleClick () {
		if (inventory.AddItem (item)) {
			Item = ItemDatabase.Instance.Nothing;
			tooltip.Hide ();
		}
	}

	private bool FilterItem (Item draggedItem) {
		if (ValidItems.Length == 0)
			return true;
		foreach (ValidItem validItem in ValidItems) {
			if (validItem.item.Id == draggedItem.Id)
				return true;
		}
		InvalidItemPlaced ();
		return false;
	}
		
}