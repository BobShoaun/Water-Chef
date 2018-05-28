using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewItemPopUp : MonoBehaviour {

	public Text text;
	
	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	private void Awake () {
		text = GetComponentsInChildren<Text> () [1];
		//visiblePosition = Vector2.zero;
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	public IEnumerator Display (Station station) {
		gameObject.SetActive (true);
		text.text = station.Title;
		yield return new WaitForSeconds (2);
		gameObject.SetActive (false);
	}

}