// ***Copyright © 2017 Doxel aka Ng Bob Shoaun. All Rights Reserved.***

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using Doxel.Utility;

public class AudioManager : SingletonMonoBehaviour<AudioManager>, ISaveLoad {

	private float masterVolume = 1;
	private float musicVolume = 1;
	private float soundEffectVolume = 1;
	private AudioPlayer[] musicPlayers;
	private const int musicPlayersCount = 2;
	private int activeMusicPlayerIndex;
	private AudioSource oneShotSoundEffectSource;
	private Dictionary<AudioPlayer, int> soundEffectPlayers;
	private int currentSoundEffectPlayId = 0;

	public float MasterVolume {
		get { return masterVolume; }
		set { 
			masterVolume = value;
			UpdateMusicVolumes ();
			UpdateSoundEffectVolumes ();
		}
	}

	public float MusicVolume {
		get { return musicVolume; }
		set { 
			musicVolume = value;
			UpdateMusicVolumes ();
		}
	}

	public float SoundEffectVolume {
		get { return soundEffectVolume; }
		set { 
			soundEffectVolume = value; 
			UpdateSoundEffectVolumes ();
		}            
	}

	protected override void Awake () {
		base.Awake ();
		musicPlayers = new AudioPlayer[musicPlayersCount];
		for (var i = 0; i < musicPlayersCount; i++)
			musicPlayers [i] = new AudioPlayer (CreateChildAudioSource ("Music Player " + i), this);
		oneShotSoundEffectSource = CreateChildAudioSource ("One Shot Sound Effect Source");
		soundEffectPlayers = new Dictionary<AudioPlayer, int> ();
	}

	public void SaveData (DataManager dataManager) {
		PlayerPrefs.SetFloat ("Master Volume", masterVolume);
		PlayerPrefs.SetFloat ("Music Volume", musicVolume);
		PlayerPrefs.SetFloat ("Sound Effect Volume", soundEffectVolume);
	}

	public void LoadData (DataManager dataManager) {
		MasterVolume = PlayerPrefs.GetFloat ("Master Volume", masterVolume);
		MusicVolume = PlayerPrefs.GetFloat ("Music Volume", musicVolume);
		SoundEffectVolume = PlayerPrefs.GetFloat ("Sound Effect Volume", soundEffectVolume);
	}

	private AudioSource CreateChildAudioSource (string gameObjectName) {
		var audioSource = new GameObject (gameObjectName).AddComponent<AudioSource> ();
		audioSource.transform.parent = transform;
		return audioSource;
	}

	public void PlayMusic (AudioClip musicClip, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		musicPlayers [activeMusicPlayerIndex].Play (musicClip, position, musicVolume * masterVolume, volumeFactor, pitch, loop);
	}

	public void PlayMusicCrossFade (float fadeDuration, AudioClip musicClip, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		musicPlayers [activeMusicPlayerIndex].StopFadeOut (fadeDuration);
		activeMusicPlayerIndex = 1 - activeMusicPlayerIndex;
		musicPlayers [activeMusicPlayerIndex].PlayFadeIn (fadeDuration, musicClip, position, musicVolume * masterVolume, volumeFactor, pitch, loop);
	}

	public void PlayMusic (string musicClipTitle, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		PlayMusic (AudioDatabase.Instance [musicClipTitle].AudioClip, position, volumeFactor, pitch, loop);
	}

	public void PlayMusicCrossFade (float fadeDuration, string musicClipTitle, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		PlayMusicCrossFade (fadeDuration, AudioDatabase.Instance [musicClipTitle].AudioClip, position, volumeFactor, pitch, loop);
	}

	public void PauseMusic () {
		musicPlayers [activeMusicPlayerIndex].Pause ();
	}

	public void UnpauseMusic () {
		musicPlayers [activeMusicPlayerIndex].Unpause ();
	}

	public void StopMusic () {
		musicPlayers [activeMusicPlayerIndex].Stop ();
	}

	public void StopMusicFadeOut (float fadeDuration) {
		musicPlayers [activeMusicPlayerIndex].StopFadeOut (fadeDuration);
	}

	private void UpdateMusicVolumes () {
		foreach (var musicPlayer in musicPlayers)
			musicPlayer.Volume = musicVolume * masterVolume;
	}

	public void PlayOneShotSoundEffect (AudioClip soundEffectClip, Vector3 position = default (Vector3), float volumeFactor = 1) {
		if (position == default (Vector3))
			oneShotSoundEffectSource.PlayOneShot (soundEffectClip, soundEffectVolume * masterVolume * volumeFactor);
		else
			AudioSource.PlayClipAtPoint (soundEffectClip, position, soundEffectVolume * masterVolume * volumeFactor);
	}

	public void PlayOneShotSoundEffect (string soundEffectClipTitle, Vector3 position = default (Vector3), float volumeFactor = 1) {
		PlayOneShotSoundEffect (AudioDatabase.Instance [soundEffectClipTitle].AudioClip, position, volumeFactor);
	}

	public int PlaySoundEffect (AudioClip soundEffectClip, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		currentSoundEffectPlayId++;
		foreach (var soundEffectPlayer in soundEffectPlayers.Keys) {
			if (!soundEffectPlayer.available)
				continue;
			soundEffectPlayer.Play (soundEffectClip, position, soundEffectVolume * masterVolume, volumeFactor, pitch, loop);
			return soundEffectPlayers [soundEffectPlayer] = currentSoundEffectPlayId;
		}
		var newSoundEffectPlayer = new AudioPlayer (CreateChildAudioSource ("Sound Effect Player " + soundEffectPlayers.Count), this);
		soundEffectPlayers.Add (newSoundEffectPlayer, currentSoundEffectPlayId);
		Debug.LogWarning ("Not enough AudioSources present to play sound effect, created new. New count : " + soundEffectPlayers.Count);
		newSoundEffectPlayer.Play (soundEffectClip, position, soundEffectVolume * masterVolume, volumeFactor, pitch, loop);
		return currentSoundEffectPlayId;
	}

	public int PlaySoundEffect (int playId, AudioClip soundEffectClip, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		if (UnpauseSoundEffect (playId))
			return playId;
		return PlaySoundEffect (soundEffectClip, position, volumeFactor, pitch, loop);
	}

	public int PlaySoundEffect (params AudioClip[] soundEffectClips) {
		return PlaySoundEffect (soundEffectClips [UnityRandom.Range (0, soundEffectClips.Length)], Vector3.zero, 1, 1, false);
	}

	public int PlaySoundEffect (params string[] soundEffectClipsTitle) {
		return PlaySoundEffect (AudioDatabase.Instance [soundEffectClipsTitle [UnityRandom.Range (0, soundEffectClipsTitle.Length)]].AudioClip, Vector3.zero, 1, 1, false);
	}

	public int PlaySoundEffect (string soundEffectClipTitle, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		return PlaySoundEffect (AudioDatabase.Instance [soundEffectClipTitle].AudioClip, position, volumeFactor, pitch, loop);
	}

	public int PlaySoundEffect (int playId, string soundEffectClipTitle, Vector3 position = default (Vector3), float volumeFactor = 1, float pitch = 1, bool loop = false) {
		return PlaySoundEffect (playId, AudioDatabase.Instance [soundEffectClipTitle].AudioClip, position, volumeFactor, pitch, loop);
	}

	public void PauseSoundEffect (int playId) {
		AudioPlayer soundEffectPlayer;
		if (TryGetSoundEffectPlayer (playId, out soundEffectPlayer))
			soundEffectPlayer.Pause ();
	}

	public bool UnpauseSoundEffect (int playId) {
		AudioPlayer soundEffectPlayer;
		if (TryGetSoundEffectPlayer (playId, out soundEffectPlayer)) {
			soundEffectPlayer.Unpause ();
			return true;
		}
		return false;
	}

	public void StopSoundEffect (int playId) {
		AudioPlayer soundEffectPlayer;
		if (TryGetSoundEffectPlayer (playId, out soundEffectPlayer))
			soundEffectPlayer.Stop ();
	}

	private bool TryGetSoundEffectPlayer (int playId, out AudioPlayer soundEffectPlayer) {
		foreach (var player in soundEffectPlayers.Keys) {
			if (player.available)
				continue;
			if (soundEffectPlayers [player] != playId)
				continue;
			soundEffectPlayer = player;
			return true;
		}
		soundEffectPlayer = null;
		return false;
	}

	private void UpdateSoundEffectVolumes () {
		foreach (var soundEffectPlayer in soundEffectPlayers.Keys)
			soundEffectPlayer.Volume = soundEffectVolume * masterVolume;
	}

	private class AudioPlayer {

		public bool available;
		private float volumeFactor;
		private float volume;
		private float playDurationRemaining;
		private bool paused;
		private AudioSource audioSource;
		private Coroutine finishCallback;
		private AudioManager audioManager;

		public float Volume {
			get { 
				return volume * volumeFactor;
			}
			set { 
				audioSource.volume = (volume = value) * volumeFactor;
			}
		}

		public AudioPlayer (AudioSource audioSource, AudioManager audioManager) {
			this.audioSource = audioSource;
			this.audioManager = audioManager;
		}

		public void Play (AudioClip audioClip, Vector3 position, float volume, float volumeFactor, float pitch, bool loop) {
			audioSource.clip = audioClip;
			audioSource.spatialBlend = position == default (Vector3) ? 0 : 1;
			audioSource.transform.position = position;
			audioSource.pitch = pitch;
			audioSource.loop = loop;
			this.volumeFactor = volumeFactor;
			Volume = volume;
			playDurationRemaining = audioClip.length;
			available = false;
			audioSource.Play ();
			finishCallback = audioManager.StartCoroutine (FinishCallback ());
		}

		public void PlayFadeIn (float fadeDuration, AudioClip audioClip, Vector3 position, float volume, float volumeFactor, float pitch, bool loop) {
			Play (audioClip, position, volume, volumeFactor, pitch, loop);
			audioManager.StartCoroutine (Utility.Fade (result => audioSource.volume = result, fadeDuration, 0, Volume));
		}

		public void Pause () {
			audioManager.StopCoroutine (finishCallback);
			playDurationRemaining = audioSource.clip.length - audioSource.time;
			audioSource.Pause ();
			paused = true;
		}

		public void Unpause () {
			if (paused) {
				paused = false;
				audioSource.UnPause ();
				finishCallback = audioManager.StartCoroutine (FinishCallback ());
			}
		}

		public void Stop () {
			if (finishCallback != null)
				audioManager.StopCoroutine (finishCallback);
			audioSource.Stop ();
			available = true;
		}

		public void StopFadeOut (float fadeDuration) {
			audioManager.StartCoroutine (Utility.Fade (result => audioSource.volume = result, fadeDuration, Volume, 0, Stop));
		}

		private void Finish () {
			if (audioSource.loop)
				finishCallback = audioManager.StartCoroutine (FinishCallback ());
			else
				available = true;
		}

		private IEnumerator FinishCallback () {
			yield return new WaitForSeconds (playDurationRemaining);
			Finish ();
		}

	}

}