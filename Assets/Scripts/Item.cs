using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Item : MonoBehaviour, IIdentifiable {

	[SerializeField]
	private int id;
	[SerializeField]
	private string title;
	public int Price;
	private RectTransform rectTransform;
	private Vector2 draggingSize;

	public int Id {
		get { return id; }
		set { id = value; }
	}

	public string Title {
		get { return title; }
	}

	private void Awake () {
		draggingSize = new Vector2 (100, 100);
		rectTransform = GetComponent<RectTransform> ();
	}

	public void ResetSize () {
		rectTransform.sizeDelta = draggingSize;
	}

	public void ResetScale () {
		transform.localScale = Vector2.one;
	}

	public override string ToString () {
		return id + " " + title;
	}

	//public static bool operator == (Item item1, Item item2) {
	//	return item1.id == item2.id;
	//}

	//public static bool operator != (Item item1, Item item2) {
	//	return !(item1 == item2);
	//}
		
}