using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Doxel.Utility;

public class CustomerSpace : MonoBehaviour, IPointerDownHandler {

	private Transform customerSpawnPoint;
	private Image image;
	public GameObject moneyCollectPrefab;
	[HideInInspector]
	public bool available;
	public Sprite alertImage;
	public Sprite coins;
	private int coinsAmount;

	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	private void Awake () {
		image = GetComponent<Image> ();
		customerSpawnPoint = transform.GetChild (0);
		available = true;
		image.enabled = false;
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	public void AssignCustomer (CustomerModule customer) {
		available = false;
		customer.transform.SetParent (customerSpawnPoint);
		customer.transform.localPosition = Vector2.zero;
		customer.transform.localScale = Vector2.one;
		customer.customerSpace = this;
	}

	public IEnumerator Alert () {
		image.sprite = alertImage;
		image.enabled = true;
		yield return new WaitForSeconds (3);
		image.enabled = false;
	}

	public void PlaceCoins (int amount) {
		StopAllCoroutines ();
		image.sprite = coins;
		image.enabled = true;
		coinsAmount = amount;
	}
		
	public void OnPointerDown (PointerEventData eventData) {
		if (coinsAmount > 0) {
			GameObject moneyCollect = Instantiate (moneyCollectPrefab, transform, false);
			moneyCollect.GetComponent<Text> ().text = "$ " + coinsAmount;
			StartCoroutine (Utility.DelayedInvokeRealTime (() => Destroy (moneyCollect), 5));
			available = true;
			image.enabled = false;
			MoneyManager.Instance.MoneyAmount += coinsAmount;
			AudioManager.Instance.PlaySoundEffect ("Ting");
			coinsAmount = 0;
		}
	
	}
}