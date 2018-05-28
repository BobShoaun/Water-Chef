using System;

[Serializable]
public class ValidItem {
	
	public Item item;
	public ItemMode itemMode;

	public ValidItem (Item item, ItemMode itemMode) {
		this.item = item;
		this.itemMode = itemMode;
	}

}
	
public enum ItemMode {
	Activate,
	Maintain,
	Deactivate
}