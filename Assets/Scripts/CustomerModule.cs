using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doxel.Utility;

public class CustomerModule : Module {

	[SerializeField]
	private AudioClip surprised;
	[SerializeField]
	private AudioClip angry;
	[SerializeField]
	private AudioClip deny;

	[NonSerialized]
	public CustomerSpace customerSpace;
	[NonSerialized]
	public bool hasOrdered;
	[NonSerialized]
	public bool hasDenied;
	public ProgressBar patienceBar;
	private TextUI speechBubble;
	private ButtonUI orderButton;
	private SpriteRenderer[] emotionSpriteRenderers;
	private Animator animator;
	private Emotion emotion;
	[SerializeField]
	private Slot orderedItemDisplaySlot;

	[NonSerialized]
	public float patience;
	private Coroutine ignoredCoroutine;

	private Item orderedItem;

	public Item OrderedItem {
		set { 
			stationSlot.ValidItems [0].item = orderedItem = value;
		}
	}

	public float Patience {
		get { return patience; }
		set { 
			Debug.Log (value);
			patience = value;
			patienceBar.speed = 1 / value;
		}
	}

	public Emotion Emotion {
		get { return emotion; }
		private set { 
			foreach (var spriteRenderer in emotionSpriteRenderers)
				spriteRenderer.enabled = false;
			switch (emotion = value) {
			case Emotion.Happy:
				emotionSpriteRenderers [0].enabled = true;
				break;
			case Emotion.Angry:
				emotionSpriteRenderers [1].enabled = true;
				break;
			case Emotion.Sad:
				emotionSpriteRenderers [2].enabled = true;
				break;
			case Emotion.Impressed:
				emotionSpriteRenderers [3].enabled = true;
				break;
			case Emotion.Unimpressed:
				emotionSpriteRenderers [4].enabled = true;
				break;
			}
		}
	}

	protected override void OnEnable () {
		base.OnEnable ();
		patienceBar.Full += () => StartCoroutine (Angry ());
		stationSlot.InvalidItemPlaced += Deny;
	}

	protected override void OnDisable () {
		base.OnDisable ();
		patienceBar.Full -= () => StartCoroutine (Angry ());
		stationSlot.InvalidItemPlaced -= Deny;
	}

	protected override void Awake () {
		base.Awake ();
		animator = GetComponent<Animator> ();
		emotionSpriteRenderers = GetComponentsInChildren<SpriteRenderer> ();
		speechBubble = GetComponentInChildren<TextUI> ();
		orderButton = GetComponentInChildren<ButtonUI> (true);
	}

	private void Start () {
		patienceBar.Deactivate ();
		hasOrdered = false;
		stationSlot.interactable = false;
		Emotion = Emotion.Happy;
	}

	protected override void Activate () {
		StopAllCoroutines ();
		orderedItemDisplaySlot.gameObject.SetActive (false);
		stationSlot.Item = ItemDatabase.Instance.Nothing;
		customerSpace.PlaceCoins (orderedItem.Price);
		stationSlot.interactable = false;
		if (hasDenied || hasOrdered) {
			speechBubble.Show ("theank u..", 3);
			//OrderList.Instance.RemoveOrder (orderedItem);
			Emotion = Emotion.Happy;
		} else {
			orderButton.Hide ();
			speechBubble.Show ("u can raed my mind??!!", 3);
			StartCoroutine (DisplayEmotion (2, Emotion.Impressed, Emotion.Happy));
			AudioManager.Instance.PlaySoundEffect (surprised);
		}
		AudioManager.Instance.PlaySoundEffect ("Tring", pitch : 1.75f - patienceBar.FillAmount / 4);
		patienceBar.Deactivate ();
		Leave ();
	}

	protected override void Deactivate () {
		Debug.Log ("customer deactivate");
	}

	private IEnumerator DisplayEmotion (float duration, Emotion first, Emotion second) {
		Emotion = first;
		yield return new WaitForSeconds (duration);
		Emotion = second;
	}

	public void ReachedCounter () {
		customerSpace.StartCoroutine (customerSpace.Alert ());
		orderButton.Show ("Take Order", Order);
		stationSlot.interactable = true;
		ignoredCoroutine = StartCoroutine (Ignored ());
	}

	private void Deny () {
		StopAllCoroutines ();
		hasDenied = true;
		orderedItemDisplaySlot.gameObject.SetActive (false);
		speechBubble.Show ("i dont want that shit", 2);
		AudioManager.Instance.PlaySoundEffect (deny);
		StartCoroutine (DisplayEmotion (2, Emotion.Unimpressed, Emotion.Happy));
		if (!hasOrdered)
			ignoredCoroutine = StartCoroutine (Ignored ());
	}

	private void Order () {
		StopCoroutine (ignoredCoroutine);
		speechBubble.Hide ();
		Emotion = Emotion.Happy;
		orderButton.Hide ();
		orderedItemDisplaySlot.gameObject.SetActive (true);
		orderedItemDisplaySlot.Item = orderedItem;
		StartCoroutine (Utility.DelayedInvoke (() => orderedItemDisplaySlot.gameObject.SetActive (false), 1.5f));
		patienceBar.Activate (1 / patience);
		//OrderList.Instance.AddOrder (orderedItem);
		hasOrdered = true;
	}

	private void Leave () {
		CustomerManager.Instance.CustomerLeaving (this);
		patienceBar.Deactivate ();
		orderButton.Hide ();
		stationSlot.interactable = false;
		animator.SetTrigger ("Leave");
	}

	public void Despawn () {
		CustomerManager.Instance.CustomerDespawned (this);
		Destroy (gameObject);
	}

	private IEnumerator Angry () {
		patienceBar.Deactivate ();
		animator.SetTrigger ("Angry");
		Emotion = Emotion.Angry;
		AudioManager.Instance.PlaySoundEffect (angry);
		speechBubble.Show ("Y u taek so long bish?!", 5);
		yield return new WaitForSeconds (5);
		//OrderList.Instance.RemoveOrder (orderedItem);
		Leave ();
	}

	private IEnumerator Ignored () {
		yield return new WaitForSeconds (patience / 5);
		Emotion = Emotion.Sad;
		speechBubble.Show ("I feel ignored :(", 10);
		yield return new WaitForSeconds (10);
		Leave ();
	}

}

public enum Emotion {
	Angry,
	Sad,
	Happy,
	Impressed,
	Unimpressed
}