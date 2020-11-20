using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
	public AudioClip initialSong;
	public AudioClip[] music;
	public AudioClip[] soundEffect;
	[HideInInspector]
	public AudioSource asmusic, assoundEffect, assoundEffect2, enemyHit;
	AudioSource[] bigAss;

	void Start () {
		bigAss = GetComponents<AudioSource> ();
		asmusic = bigAss [0];
		assoundEffect = bigAss [1];
		assoundEffect2 = bigAss [2];
		enemyHit = bigAss [3];
		assoundEffect2.volume = assoundEffect.volume;
		asmusic.clip = initialSong;
		asmusic.Play ();
	}
	

	public void PlaySound(string sound){
		if (sound == "Attack") {
			int random = Random.Range (1, 4);
			switch (random) {
			case 1:
				assoundEffect.clip = soundEffect [0];
				break;
			case 2:
				assoundEffect.clip = soundEffect [1];
				break;
			case 3:
				assoundEffect.clip = soundEffect [2];
				break;
			}
			assoundEffect.Play ();
		} 

		else if (sound == "Dash") {
			int random = Random.Range (1, 5);
			switch (random) {
			case 1:
				assoundEffect2.clip = soundEffect [3];
				break;
			case 2:
				assoundEffect2.clip = soundEffect [4];
				break;
			case 3:
				assoundEffect2.clip = soundEffect [5];
				break;
			case 4:
				assoundEffect2.clip = soundEffect [6];
				break;
			}
			assoundEffect2.Play ();
		}

		else if (sound == "THE LOG") {
			enemyHit.clip = soundEffect [7];
			enemyHit.Play ();
		} 

		else if (sound == "Slime") {
			enemyHit.clip = soundEffect [8];
			enemyHit.Play ();
		}

	}

	public void PlayMusic(AudioClip music = null, bool stop = false){
		if (music != null) {
			asmusic.clip = music;
			asmusic.Play ();
		} else if (stop) {
			asmusic.Stop ();
		}
			
	}
}
