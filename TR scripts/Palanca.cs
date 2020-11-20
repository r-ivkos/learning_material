using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour {
	private Animator anim;
	private Piedrita Piedri;

	void Start(){
		anim = GetComponent<Animator> ();
		Piedri = FindObjectOfType<Piedrita> ();
	}



	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player" && Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Activar", true);
			Piedri.Activado = true;
		}
	}
}
