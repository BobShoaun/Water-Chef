using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour {

	private Text orderText;
	private Text orderAmountText;
	private Item orderedItem;
	private int orderAmount;

	public int OrderAmount {
		get{ 
			return orderAmount;
		}
		set { 
			orderAmount = value;
			UpdateUI();
		}
	}

	public Item OrderedItem {
		get {
			return orderedItem;
		}
		set { 
			orderedItem = value;
			UpdateUI();
		}
	}
	void Awake () {
		orderText = GetComponentInChildren<Text> ();
		orderAmountText = GetComponentsInChildren<Text> () [1];
	}

	private void UpdateUI () {
		orderText.text = orderedItem.Title;
		orderAmountText.text = "x " + orderAmount;
	}
		
}