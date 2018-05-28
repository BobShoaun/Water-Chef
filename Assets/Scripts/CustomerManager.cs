using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : SemiSingletonMonoBehaviour<CustomerManager> {

	[SerializeField]
	private GameObject[] customerPrefabs;
	private CustomerSpace[] customerSpaces;
	private List<CustomerModule> spawnedCustomers;
	private bool canSpawn;

	[HideInInspector]
	public float customerSpawnInterval;
	[HideInInspector]
	public float customerPatience;
	[HideInInspector]
	public string[] customerOrderedItems;

    public int satisfiedCustomerCount;
    public int guessedCustomerCount;
    public bool perfectCustomerSatisfaction;

	public float CustomerPatience {
		get { return customerPatience; }
		set { 
			customerPatience = value;
			foreach (var customer in spawnedCustomers)
				customer.Patience = Random.Range (customerPatience - 3, customerPatience + 3);
		}
	}

	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	protected override void Awake () {
		base.Awake ();
		customerSpaces = GetComponentsInChildren<CustomerSpace> ();
		spawnedCustomers = new List<CustomerModule> ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

	private IEnumerator SpawnCustomer () {
		while (canSpawn) {
			for (var i = 0; i < customerSpaces.Length; i++) {
				CustomerSpace customerSpace = customerSpaces [Random.Range (0, customerSpaces.Length)];
				if (customerSpace.available) {
					CustomerModule spawnedCustomer = Instantiate (customerPrefabs [Random.Range (0, customerPrefabs.Length)]).GetComponent<CustomerModule> ();
					spawnedCustomer.patience = Random.Range (customerPatience - 3, customerPatience + 3);
					spawnedCustomer.OrderedItem = ItemDatabase.Instance [customerOrderedItems [Random.Range (0, customerOrderedItems.Length)]];
					customerSpace.AssignCustomer (spawnedCustomer);
					spawnedCustomers.Add (spawnedCustomer);
					break;
				}
			}
			var randomFactor = customerSpawnInterval * 0.2f;
			yield return new WaitForSeconds (Random.Range (customerSpawnInterval - randomFactor, customerSpawnInterval + randomFactor));
		}
	}

	public void StartSpawning () {
		canSpawn = true;
		StartCoroutine (SpawnCustomer ());
	}

	public void StopSpawning () {
		canSpawn = false;
	}

    public void CustomerLeaving (CustomerModule customer) {
		if (!customer.hasDenied && !customer.hasOrdered)
            guessedCustomerCount++;
		if (customer.Emotion == Emotion.Happy || customer.Emotion == Emotion.Impressed)
            satisfiedCustomerCount++;
        else
            perfectCustomerSatisfaction = false;
    }

	public void CustomerDespawned (CustomerModule customer) {
		spawnedCustomers.Remove (customer);
	}

    private void DespawnAll () {
		for (var i = 0; i < spawnedCustomers.Count; i++)
			spawnedCustomers [i].Despawn ();
	}

    public void Reset () {
        DespawnAll ();
        guessedCustomerCount = 0;
        satisfiedCustomerCount = 0;
        perfectCustomerSatisfaction = true;
    }

}