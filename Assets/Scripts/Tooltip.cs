using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	private Text tipMessage;
	private bool isVisible;
	private Vector2 invisiblePosition;

	private bool IsVisible {
		set { 
			transform.position = (isVisible = value) ? Input.mousePosition : (Vector3) invisiblePosition;
		}
	}

	private void Awake () {
		tipMessage = GetComponentInChildren<Text> ();
		invisiblePosition = new Vector2 (0, 1000);
	}

	private void Start () {
		IsVisible = false;
	}

	private void LateUpdate () {
		if (isVisible)
			transform.position = Input.mousePosition;
	}

	public void Show (Item item) {
		if (item.Id != 0) {
			tipMessage.text = item.Title;
			IsVisible = true;
		}
	}

	public void Hide () {
		IsVisible = false;
	}

}