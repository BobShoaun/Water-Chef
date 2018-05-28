using UnityEngine;

public class StationDatabase : Database<Station, StationDatabase> {

	protected override void Awake () {
		base.Awake ();
		Elements = FindObjectsOfType<Station> ();
		//foreach (var element in Elements)
		//	element.gameObject.SetActive (false);
	}

}