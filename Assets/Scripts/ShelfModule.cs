using UnityEngine;

public class ShelfModule : Module {

	private Item displayedItem;
	private Inventory playerInventory;

	protected override void OnEnable () {
		base.OnEnable ();
		stationSlot.PointerDown += OnPointerDown;
	}
	
	protected override void OnDisable () {
		base.OnDisable ();
		stationSlot.PointerDown -= OnPointerDown;
	}
	
	protected override void Awake () {
		base.Awake ();
	}

	private void Start () {
		displayedItem = stationSlot.ValidItems [0].item;
		playerInventory = FindObjectOfType<Inventory> ();
	}

	private void Update () {
		
	}

	protected override void Activate () {
		ReplaceItem (stationSlot, ItemDatabase.Instance.Nothing);
	}

	protected override void Deactivate () {

	}

	public void OnPointerDown () {
		playerInventory.AddItem (displayedItem);
	}

}