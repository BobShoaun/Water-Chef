using System;
using UnityEngine;

public class DayManager : SingletonMonoBehaviour<DayManager> {

	public event Action<Level> OnDayChanged = delegate {};
	public event Action<Level> OnDayEnd = delegate {};
	private Level currentDay;

	public Level CurrentDay {
		get { return currentDay; }
		set { 
			currentDay = value;
			OnDayChanged (value);
		}
	}

	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	protected override void Awake () {
		base.Awake ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	/*public void StartDay () {
		StartCoroutine (currentDay.Start (EndDay));
		CustomerManager.Instance.satisfiedCustomerCount = 0;
		CustomerManager.Instance.spawner.StartSpawning ();
	}

	private void EndDay () {
		CustomerManager.Instance.spawner.StopSpawning ();
		currentDay.success = CustomerManager.Instance.satisfiedCustomerCount >= currentDay.customersToSatisfy;
		if (currentDay.success)
			FinishDay ();
		OnDayEnd (currentDay);
	}

	public void NextDay () {
		CurrentDay = LevelDatabase.Instance.QueryById (currentDay.Id + 1);
	}

	private void FinishDay () {
		
	}*/

}