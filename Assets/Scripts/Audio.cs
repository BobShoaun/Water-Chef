using System;
using UnityEngine;

[Serializable]
public class Audio : IIdentifiable {

	[SerializeField]
	private int id;
	[SerializeField]
	private string title;
	[SerializeField]
	private AudioClip audioClip;

	public int Id {
		get { return id; }
		set { id = value; }
	}

	public string Title {
		get { return title; }
	}

	public AudioClip AudioClip {
		get { return audioClip; }
	}

}