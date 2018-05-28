using UnityEngine;

public class LevelDatabase : Database<Level, LevelDatabase> {
	
	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	protected override void Awake () {
		base.Awake ();
		/*Elements = new Level[] {
			new Level (0, "Day 1", new Goal[] {
				new Goal (GoalType.SatisfyCustomers, 2, "Satisfy {0} Customers", 3)
			}, new WaitForSeconds (60)),

			new Level (1, "Day 2", new Goal[] {
				new Goal (GoalType.EarnMoney, 100, "Earn {0} Bucks", 3)
			}, new WaitForSeconds (120)),

			new Level (2, "Day 3", new Goal[] {
				new Goal (GoalType.SatisfyCustomers, 2, "Satisfy {0} Customers", 1),
				new Goal (GoalType.EarnMoney, 100, "Earn {0} Bucks", 2)
			}, new WaitForSeconds (200)),

			new Level (3, "Day 4", new Goal[] {
				new Goal (GoalType.SatisfyCustomers, 2, "Satisfy {0} Customers", 3)
			}, new WaitForSeconds (60)),
		};*/
	}

	private void Start () {
	}

	private void Update () {
		
	}

}

