using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IIdentifiable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public event Action ItemChanged = delegate {};
	protected static Item draggedItem;

	[SerializeField]
	private int id;
	public bool interactable = true;
	protected Item item;
	private Transform screenCameraCanvas;
	protected Tooltip tooltip;
	private Image image;
	private Color initialColor;
	[SerializeField]
	private Color32 hoverColor = new Color32 (211, 211, 211, 255);
	private bool pointerOver;

	public string Title { get; }

	public int Id {
		get { return id; }
		set { id = value; }
	}
		
	public Item Item {
		get { return item; } 
		set { 
			Destroy (item.gameObject);
			item = Instantiate (value, transform);
			item.ResetScale ();
			if (pointerOver)
				OnPointerEnter (null);
			ItemChanged ();
		}
	}

	protected virtual void Awake () {
		item = GetComponentInChildren<Item> ();
		screenCameraCanvas = GameObject.FindGameObjectWithTag ("Screen Camera Canvas").transform;
		tooltip = FindObjectOfType<Tooltip> ();
		image = GetComponent<Image> ();
		initialColor = image.color;
	}

	private void Start () {
		//ItemChanged ();
	}

	private void Update () {
		if (draggedItem != null)
			Dragging ();
	}
		
	public void OnPointerEnter (PointerEventData eventData) {
		if (!interactable)
			return;
		pointerOver = true;
		image.color = hoverColor;
		if (draggedItem == null)
			tooltip.Show (item);
	}
		
	public void OnPointerExit (PointerEventData eventData) {
		pointerOver = false;
		image.color = initialColor;
		tooltip.Hide ();
	}

	public virtual void OnPointerDown (PointerEventData eventData) {
		if (!interactable)
			return;
		if (draggedItem == null) {
			if (item.Id != 0)
				BeginDrag ();
			return;
		}
		if (item.Id == 0)
			EndDrag ();
		else
			DragSwitch ();
	}

	protected void BeginDrag () {
		draggedItem = Instantiate (ItemDatabase.Instance [item.Id], screenCameraCanvas);
		draggedItem.ResetScale ();
		draggedItem.ResetSize ();
		Item = ItemDatabase.Instance.Nothing;
		tooltip.Hide ();
		AudioManager.Instance.PlayOneShotSoundEffect ("Blop");
	}

	protected void Dragging () {
		draggedItem.transform.position = (Vector2) Camera.allCameras [0].ScreenToWorldPoint (Input.mousePosition);
	}

	protected void EndDrag () {
		Item = ItemDatabase.Instance [draggedItem.Id];
		Destroy (draggedItem.gameObject);
		tooltip.Show (item);
		AudioManager.Instance.PlaySoundEffect ("Blop", volumeFactor : 0.7f, pitch : 0.9f);
	}

	protected void DragSwitch () {
		var itemId = Item.Id;
		Item = ItemDatabase.Instance [draggedItem.Id];
		Destroy (draggedItem.gameObject);
		draggedItem = Instantiate (ItemDatabase.Instance [itemId], screenCameraCanvas);
		draggedItem.ResetScale ();
		draggedItem.ResetSize ();
		AudioManager.Instance.PlaySoundEffect ("Blop", volumeFactor : 0.7f);
	}

}