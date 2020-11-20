using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour {

	public Player_Movement pm; 
	private Rigidbody2D rb2d;
	ParticleSystem.EmissionModule em;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		em = GetComponent <ParticleSystem> ().emission; 
		em.enabled = false;


	}


	void FixedUpdate () {
		transform.position = pm.transform.position;
		em.enabled = false;
		if (pm.dashTime <= 0) {
			rb2d.velocity = Vector2.zero;
			em.enabled = false;
		} else {
			if (pm.DashDirection == 1) {
				em.enabled = true;
				rb2d.velocity = Vector2.right * pm.DashSpeed;
			} else if (pm.DashDirection == 2) {
				em.enabled = true;
				rb2d.velocity = Vector2.left * pm.DashSpeed;
			} else if (pm.DashDirection == 3) {
				em.enabled = true;
				rb2d.velocity = Vector2.up * pm.DashSpeed;
			} else if (pm.DashDirection == 4) {
				em.enabled = true;
				rb2d.velocity = Vector2.down * pm.DashSpeed;
			}
		}
	}
}