using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private Slot[] slots;

	private void Awake () {
		slots = GetComponentsInChildren<Slot> ();
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			AddItem (ItemDatabase.Instance ["Cup Of Water"]);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			AddItem (ItemDatabase.Instance ["Pot Of Boiled Water"]);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			AddItem (ItemDatabase.Instance ["Pot Of Water"]);
		}
	}

	public bool AddItem (Item item) {
		foreach (var slot in slots) {
			if (slot.Item.Id != 0)
				continue;
			slot.Item = item;
			return true;
		}
		Debug.Log ("Inventory is full");
		return false;
	}
		
}
