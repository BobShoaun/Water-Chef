using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour {

	private TextUI moneyText;

	private void OnEnable() {
		MoneyManager.Instance.MoneyAmountChanged += UpdateUI;
	}

	private void OnDisable() {
		MoneyManager.Instance.MoneyAmountChanged -= UpdateUI;
	}

	private void Awake() {
		moneyText = GetComponent<TextUI> ();
	}

	private void Start () {
		UpdateUI ();
	}

	private void UpdateUI () {
		moneyText.Show ("$ " + MoneyManager.Instance.MoneyAmount);
		moneyText.TextColor = MoneyManager.Instance.MoneyAmount < 0 ? Color.red : moneyText.initialColor;
	}

}