using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderList : MonoBehaviour {

	public Transform orderPanel;
	public Order orderPrefab;
	private List<Order> orders;
	private Button orderListButton;
	private Vector2 invisiblePosition;
	private Vector2 visiblePosition;
	private bool isVisible;
	private RectTransform rectTransform;

	public static OrderList Instance { get; private set; }

	public bool IsVisible {
		get { return isVisible; }
		set { 
			isVisible = value;
			rectTransform.anchoredPosition = value ? visiblePosition : invisiblePosition;
		}
	}

	private void OnDisable () {
		orderListButton.onClick.RemoveListener (() => IsVisible = !IsVisible);
	}
		
	private void Awake () {
		Instance = this;
		rectTransform = GetComponent<RectTransform> ();
		orderListButton = GetComponentInChildren<Button> ();
		orders = new List<Order> ();
		visiblePosition = Vector2.zero;
		invisiblePosition = rectTransform.anchoredPosition;
	}

	private void Start () {
		IsVisible = false;
		orderListButton.onClick.AddListener (() => IsVisible = !IsVisible);
	}

	public void AddOrder (Item item) {
		foreach (var order in orders) {
			if (order.OrderedItem.Id != item.Id)
				continue;
			order.OrderAmount++;
			return;
		}
		Order newOrder = Instantiate (orderPrefab, orderPanel);
		newOrder.transform.localScale = Vector2.one;
		newOrder.OrderedItem = item;
		newOrder.OrderAmount++;
		orders.Add (newOrder);
	}

	public void RemoveOrder (Item item) {
		foreach (var order in orders) {
			if (order.OrderedItem.Id != item.Id)
				continue;
			order.OrderAmount--;
			if (order.OrderAmount > 0)
				return;
			orders.Remove (order);
			Destroy (order.gameObject);
			return;
		}
		Debug.LogError ("No order to remove!");
	}

}