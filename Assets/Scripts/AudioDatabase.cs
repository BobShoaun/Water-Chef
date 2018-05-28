using UnityEngine;

public class AudioDatabase : Database<Audio, AudioDatabase> {

	protected override void Awake () {
		base.Awake ();
	}

}

[System.Serializable]
public class AudioGroup : IIdentifiable {

	[SerializeField]
	private string title;

	public int Id { get; set; }
	public string Title {
		get {
			return title;
		}
	}
	public AudioClip[] audioClips;

	public AudioGroup () {
		
	}

}