using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour {

	// Use this for initialization

	private Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
	}



	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Fire") {
			anim.SetTrigger ("Fire");

		}
	}
}

