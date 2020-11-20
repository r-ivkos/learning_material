using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruccion : MonoBehaviour {

	public string destroyState;
	public float timeForDisable;

	Animator anim;
	 
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo si = anim.GetCurrentAnimatorStateInfo (0);
		if (si.IsName (destroyState) && si.normalizedTime >= 1) {
			Destroy (gameObject);
		}
	}
		
	IEnumerator OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Sword" || other.tag == "Fire") {
		
			anim.Play (destroyState);
			yield return new WaitForSeconds(timeForDisable);

			foreach(Collider2D c in GetComponents<Collider2D>()){
				c.enabled = false;

			}
		}
	}
	
}
