using UnityEngine;

public class Shop : MonoBehaviour {

	private ButtonUI buttonUI;
	private Item item;
	private TextUI priceTag;
	private Inventory playerInventory;

	private void Awake () {
		item = GetComponentInChildren<Item> ();
		buttonUI = GetComponentInChildren<ButtonUI> ();
		priceTag = GetComponentInChildren<TextUI> ();
		playerInventory = FindObjectOfType<Inventory> ();


	}

	private void Start () {
		//Debug.Log ("shop activate");
		buttonUI.Show ("Buy " + item.Title, PurchaseItem);
		priceTag.Show ("$ " + item.Price);
	}

	private void PurchaseItem() {
		if (playerInventory.AddItem (item)) {
			MoneyManager.Instance.MoneyAmount -= item.Price;
		} else {
			
		}
	}
}
