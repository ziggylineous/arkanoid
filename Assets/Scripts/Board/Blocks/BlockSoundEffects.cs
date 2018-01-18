using System.Collections.Generic;
using UnityEngine;

public class BlockSoundEffects : MonoBehaviour {

	List<AudioSource> soundPlayers;

	void Start() {
		soundPlayers = new List<AudioSource> ();
		GetComponents<AudioSource> (soundPlayers);
	}

	public void Play (AudioClip clip) {
		AudioSource firstNotPlaying = soundPlayers.Find (soundSource => !soundSource.isPlaying);

		if (firstNotPlaying == null) {
			firstNotPlaying = soundPlayers [soundPlayers.Count - 1];
			firstNotPlaying.Stop ();
		}

		soundPlayers.Remove (firstNotPlaying);
		soundPlayers.Insert (0, firstNotPlaying);

		firstNotPlaying.clip = clip;
		firstNotPlaying.Play ();
	}
}
