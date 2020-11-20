using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour {
	private Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
	}



	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player" && Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Abrir", true);
		}
	}
}
