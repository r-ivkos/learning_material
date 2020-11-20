using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piedrita : MonoBehaviour {
	
	public bool Activado = false;
	private Animator anim;
	private BoxCollider2D box;


	void Start(){
		anim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider2D> ();
	}

	void Update () {
		if (Activado == true) {
			anim.SetBool ("Activar", true);
			Destroy (box);
		}	
	}
}
