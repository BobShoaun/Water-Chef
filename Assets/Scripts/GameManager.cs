using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Doxel.Utility;

public class GameManager : SingletonMonoBehaviour<GameManager>, ISaveLoad {

	public event Action<Level> LevelEnded = delegate {};
	public event Action LevelStarted = delegate {};

	public AudioClip mainTheme;
	public AudioClip backPocket;
	public AudioClip kitchenTheme;
	[NonSerialized]
	public Level[] levels;
	[NonSerialized]
	public Level currentLevel;

	private bool inKitchenScene;
	private bool paused;

	private GameObject countdown;
	private GameObject timesUpMessage;
	private PauseMenu pauseMenu;
	private LevelMenu levelMenu;
	private ProgressBar levelTimer;
	private PlayerController player;
	private GameObject preventInteractionPanel;
	private NewItemPopUp newItemPopUp;
	private Station[] stations;

	private void OnEnable () {
		SceneController.Instance.SceneUnload += OnSceneUnload;
		SceneController.Instance.SceneLoaded += OnSceneLoaded;
		SceneController.Instance.ApplicationStart += OnApplicationStart;
		SceneController.Instance.ApplicationQuit += OnApplicationQuit;
	}

	private void OnDisable () {
		SceneController.Instance.SceneUnload -= OnSceneUnload;
		SceneController.Instance.SceneLoaded -= OnSceneLoaded;
		SceneController.Instance.ApplicationStart -= OnApplicationStart;
		SceneController.Instance.ApplicationQuit -= OnApplicationQuit;
	}

	protected override void Awake () {
		base.Awake ();
	}

	private void Start () {

	}

	private void CreateLevels () {
		levels = new [] {

			new Level (0, "Day 1", new [] {
				new Goal (GoalType.SatisfyCustomers, 2, 3)
			}, 60, new [] { "Counter", "Shelf", "Sink", "Stove", "Pouring Mat" },
				false, 10, 30, new [] { "Cup Of Water" }),

			new Level (1, "Day 2", new [] {
				new Goal (GoalType.EarnMoney, 100, 3)
			}, 200, new [] { "Counter", "Shelf", "Sink", "Stove", "Pouring Mat" },
				false, 20, 30, new [] { "Cup Of Water" }),

			new Level (2, "Day 3", new [] {
				new Goal (GoalType.SatisfyCustomers, 2, 1),
				new Goal (GoalType.EarnMoney, 100, 2)
			}, 100, new [] {
				"Counter",
				"Shelf",
				"Sink",
				"Stove",
				"Pouring Mat",
				"Trash Can"
			},
				true, 20, 40, new [] { "Cup Of Water" }),

			new Level (3, "Day 4", new [] {
				new Goal (GoalType.SatisfyCustomers, 5, 1),
				new Goal (GoalType.EarnMoney, 200, 2)
			}, 60, new [] {
				"Counter",
				"Shelf",
				"Sink",
				"Stove",
				"Pouring Mat",
				"Trash Can",
				"Ice Dispenser"
			},
				true, 10, 10, new [] { "Cup Of Water", "Cup Of Ice Water" }),

		};
		currentLevel = levels [0]; //temp
		currentLevel.unlocked = true;
	}

	public void SaveData (DataManager dataManager) {
		dataManager.Save ("Levels", levels);
	}

	public void LoadData (DataManager dataManager) {
		if (!dataManager.Load ("Levels", ref levels))
			CreateLevels ();
	}

	int playId1;
	int playId2;

	private void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			playId2 = AudioManager.Instance.PlaySoundEffect (playId2, "Slurp", loop: true);
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			AudioManager.Instance.PauseSoundEffect (playId2);
		}
		if (Input.GetKeyDown (KeyCode.J)) {
			AudioManager.Instance.UnpauseSoundEffect (playId2);
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			AudioManager.Instance.StopSoundEffect (playId2);
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			playId1 = AudioManager.Instance.PlaySoundEffect (playId1, "Boiling", loop: true);
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			AudioManager.Instance.PauseSoundEffect (playId1);
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			AudioManager.Instance.UnpauseSoundEffect (playId1);
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			AudioManager.Instance.StopSoundEffect (playId1);
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			AudioManager.Instance.PlayMusicCrossFade (5, mainTheme);
		}

		if (inKitchenScene && Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown (KeyCode.Escape)) {
			if (paused)
				Unpause ();
			else
				Pause ();
		}

	}

	private void Pause () {
		pauseMenu.Show ();
		paused = true;
		Time.timeScale = 0;
	}

	public void Unpause () {
		Time.timeScale = 1;
		pauseMenu.Hide ();
		paused = false;
	}

	private void OnApplicationStart () {
		DataManager.Instance.LoadFromFile ();
		DataManager.Instance.LoadAll ();
	}

	private void OnSceneLoaded () {
		DataManager.Instance.LoadAll ();
		var reference = FindObjectOfType<KitchenSceneReference> ();
		if (inKitchenScene = reference != null)
			InitializeLevel (reference);
	}

	private void OnSceneUnload () {
		StopAllCoroutines ();
		DataManager.Instance.SaveAll ();
	}

	private void OnApplicationQuit () {
		DataManager.Instance.SaveAll ();
		DataManager.Instance.SaveToFile ();
	}

	private void InitializeLevel (KitchenSceneReference levelReference) {
		InitializeReferences (levelReference);
		if (currentLevel.newStation && newItemPopUp != null) {
			//yield return StartCoroutine (newItemPopUp.Display (StationDatabase.Instance [currentLevel.stations [currentLevel.stations.Length - 1]]));
			currentLevel.newStation = false;
		}
		CustomerManager.Instance.customerOrderedItems = currentLevel.customerOrderedItems;
		CustomerManager.Instance.customerSpawnInterval = currentLevel.customerSpawnInterval;
		CustomerManager.Instance.customerPatience = currentLevel.customerPatience;
		ResetLevel ();
		levelMenu.DisplayLevel (currentLevel);
		AudioManager.Instance.PlayMusic (kitchenTheme, loop : true);
	}

	private void InitializeReferences (KitchenSceneReference kitchenSceneReferences) {
		levelMenu = kitchenSceneReferences.levelMenu;
		pauseMenu = kitchenSceneReferences.pauseMenu;
		player = kitchenSceneReferences.player;
		levelTimer = kitchenSceneReferences.levelTimer;
		countdown = kitchenSceneReferences.countdown;
		timesUpMessage = kitchenSceneReferences.timesUpMessage;
		preventInteractionPanel = kitchenSceneReferences.preventInteractionPanel;
		newItemPopUp = kitchenSceneReferences.newItemPopUp;
		stations = FindObjectsOfType<Station> ();
	}

	public IEnumerator StartLevel () {
		levelMenu.Hide ();
		ResetLevel ();
		yield return new WaitForSeconds (0.5f);
		countdown.SetActive (true);
		yield return new WaitForSeconds (3);
		countdown.SetActive (false);
		levelTimer.Reset ();
		levelTimer.Activate (1 / currentLevel.length);
		CustomerManager.Instance.StartSpawning ();
		foreach (var station in stations)
			station.canEnterAndExit = true;
		player.canMove = true;
		preventInteractionPanel.SetActive (false);
		StartCoroutine (EndLevel ());
	}

	private IEnumerator EndLevel () {
		yield return new WaitForSeconds (currentLevel.length - 10);
		var tickPlayid = AudioManager.Instance.PlaySoundEffect ("Clock Tick");
		yield return new WaitForSeconds (10);
		AudioManager.Instance.StopSoundEffect (tickPlayid);
		preventInteractionPanel.SetActive (true);
		foreach (var station in stations)
			station.canEnterAndExit = false;
		player.canMove = false;
		CustomerManager.Instance.StopSpawning ();
		currentLevel.CalculatePoints ();
		var timerAnimator = levelTimer.GetComponent<Animator> ();
		timerAnimator.SetBool ("Alarm", true);
		var ringPlayId = AudioManager.Instance.PlaySoundEffect ("Ring");
		yield return new WaitForSeconds (3);
		AudioManager.Instance.StopSoundEffect (ringPlayId);
		timerAnimator.SetBool ("Alarm", false);
		AudioManager.Instance.PlaySoundEffect ("Time's Up");
		timesUpMessage.SetActive (true);
		yield return new WaitForSeconds (3);
		timesUpMessage.SetActive (false);
		if (currentLevel.starAmount > 0)
			levelMenu.DisplaySuccess (currentLevel);
		else
			levelMenu.DisplayFail (currentLevel);
		Debug.Log ("satisfied: " + CustomerManager.Instance.satisfiedCustomerCount);
		Debug.Log ("Guessed " + CustomerManager.Instance.guessedCustomerCount);
		Debug.Log ("perfect " + CustomerManager.Instance.perfectCustomerSatisfaction);
		Debug.Log ("Profit: " + MoneyManager.Instance.Profit);
	}

	public void NextLevel () {
		levelMenu.Hide ();
		currentLevel.played = true;
		if (currentLevel.Id + 1 < levels.Length)
			currentLevel = levels [currentLevel.Id + 1];
		currentLevel.unlocked = true;
		SceneController.Instance.LoadSceneFade ("Level Select");
	}

	private void ResetLevel () {
		MoneyManager.Instance.CacheMoney ();
		CustomerManager.Instance.Reset ();
		foreach (var station in stations) {
			if (station.entered)
				station.Exit ();
		}
	}

}

[Serializable]
public class Level : IIdentifiable {

	public int starAmount;
	public bool unlocked;
	public Goal[] goals;
	public float length;
	public string[] stations;
	public bool newStation;

	public bool played;

	public float customerSpawnInterval;
	public float customerPatience;
	public string[] customerOrderedItems;

	public int Id { get; set; }

	public string Title { get; private set; }

	public Level (int id, string title, Goal[] goals, float length, string[] stations,
	              bool newStation, float customerSpawnInterval, float customerPatience,
	              string[] customerOrderedItems) {
		starAmount = 0;
		unlocked = false;
		this.goals = goals;
		this.length = length;
		Id = id;
		Title = title;
		this.stations = stations;
		this.newStation = newStation;
		this.customerSpawnInterval = customerSpawnInterval;
		this.customerPatience = customerPatience;
		played = false;
		this.customerOrderedItems = customerOrderedItems;
	}

	public int CalculatePoints () {
		for (var i = 0; i < goals.Length; i++) {
			if (goals [i].Achieved)
				starAmount += goals [i].starReward;
		}
		return starAmount;
	}

}

[Serializable]
public class Goal {

	private GoalType goalType;
	private int value;
	[Range (1, 3)]
	public int starReward;

	public bool Achieved {
		get {
			switch (goalType) {
			case GoalType.SatisfyCustomers:
				return CustomerManager.Instance.satisfiedCustomerCount >= value;
			case GoalType.EarnMoney:
				return MoneyManager.Instance.Profit >= value;
			case GoalType.GuessCustomers:
				return CustomerManager.Instance.guessedCustomerCount >= value;
			case GoalType.PerfectCustomerSatisfaction:
				return CustomerManager.Instance.perfectCustomerSatisfaction;
			default :
				return false;
			}
		}
	}

	public string Description {
		get {
			switch (goalType) {
			case GoalType.EarnMoney:
				return string.Format ("Earn {0} Bucks", value);
			case GoalType.SatisfyCustomers:
				return string.Format ("Satisfy {0} Customers", value);
			case GoalType.GuessCustomers:
				return string.Format ("Guess {0} Customer's Order", value);
			case GoalType.PerfectCustomerSatisfaction:
				return "Satisfy All Customers";
			default :
				return string.Empty;
			}
		}
	}

	public Goal (GoalType goalType, int value, int starReward) {
		this.goalType = goalType;
		this.value = value;
		this.starReward = starReward;
	}

}

public enum GoalType {
	SatisfyCustomers,
	EarnMoney,
	GuessCustomers,
	NoKitchenMishaps,
	PerfectCustomerSatisfaction
}