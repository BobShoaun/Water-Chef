using UnityEngine;
using UnityEngine.UI;

public class GoalUI : MonoBehaviour {

	private Text goalMessage;
	private Toggle checkMark;
	//public Goal goal;

	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	private void Awake () {
		checkMark = GetComponent<Toggle> ();
		goalMessage = GetComponentInChildren<Text> ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	public void Display (Goal goal) {
		//this.goal = goal;
		checkMark.isOn = goal.Achieved;
		goalMessage.text = goal.Description;
	}

}