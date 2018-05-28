using System;
using UnityEngine;

public class MoneyManager : SemiSingletonMonoBehaviour<MoneyManager>, ISaveLoad {

	[SerializeField]
	private int startingMoneyAmount;
	private int moneyAmount;
	public event Action MoneyAmountChanged = delegate {};
	private int startLevelMoney;

	public int MoneyAmount {
		get { return moneyAmount; }
		set { 
			moneyAmount = value;
			MoneyAmountChanged ();
		}
	}

	public int Profit {
		get { 
			return moneyAmount - startLevelMoney;
		}
	}

	private void OnEnable () {

	}

	public void CacheMoney () {
		startLevelMoney = moneyAmount;
	}

	public void SaveData (DataManager dataManager) {
		dataManager.Save ("Money Amount", moneyAmount);
	}

	public void LoadData (DataManager dataManager) {
		if (!dataManager.Load ("Money Amount", ref moneyAmount))
			MoneyAmount = startingMoneyAmount;
	}

}