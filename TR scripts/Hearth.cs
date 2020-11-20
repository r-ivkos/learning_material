using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour {

	HealthBar Hub;
	Player_Movement Pm;
	public float Heal;

	void Start () {
		Hub = FindObjectOfType <HealthBar> ();
		Pm = FindObjectOfType <Player_Movement> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			if (Hub.hp != Hub.maxHp) {
				Hub.Heal (Heal);
				Pm.hp = Mathf.Clamp (Pm.hp + Heal, 0, Pm.maxHP);
				Destroy (gameObject);
			}
		}
	}
}
