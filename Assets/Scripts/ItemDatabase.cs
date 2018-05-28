public class ItemDatabase : Database<Item, ItemDatabase> {

	public Item Nothing {
		get { 
			return this [0];
		}
	}

}