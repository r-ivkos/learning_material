using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour {
	public float delay;

	Animator anim;
	Coroutine manager;
	bool charged;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
	}

	public void AuraStart() {
		manager = StartCoroutine (Manager ());
		anim.Play ("None");
	}

	public void AuraStop() {
		StopCoroutine (manager);
		anim.Play ("None");
		charged = false;
	}

	public IEnumerator Manager(){
		yield return new WaitForSeconds (delay);
		anim.Play ("Aura");
		charged = true;
	}
	public bool IsCharged() {
		return charged;
	}
}