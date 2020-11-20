using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampilla : MonoBehaviour {
	private Animator anim;
	private BoxCollider2D box;

	void Start(){
		anim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider2D> ();
	}



	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player" && Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Abrir", true);
			Destroy (box);
			}
		}
	}
