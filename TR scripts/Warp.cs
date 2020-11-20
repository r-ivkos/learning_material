using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	public GameObject target;
	public GameObject targetMap;
	public AudioClip targetSong;
	private SoundController sound;
	void Awake(){
		GetComponent<SpriteRenderer> ().enabled = false;
		transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;
		sound = GameObject.Find ("Sound Controller").GetComponent<SoundController> ();

	}

	void Update(){
		
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			other.transform.position = target.transform.GetChild(0).transform.position;
			GameObject.Find("Kamera").GetComponent<MainCamera>().BORDES(targetMap);
			if (sound.asmusic.clip != targetSong || !sound.asmusic.isPlaying)
				sound.PlayMusic (targetSong);
			if (targetSong == null) {
				sound.PlayMusic (null, true);
			}
		}
	}
}
