using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour {

	HealthBar Hub;
	Player_Movement Pm;
	public float Heal_points;

	void Start () {
		Hub = FindObjectOfType <HealthBar> ();
		Pm = FindObjectOfType <Player_Movement> ();

	}

	public bool Heal(){
		if (Hub.hp != Hub.maxHp) {
			Hub.Heal (Heal_points);
			Pm.hp = Mathf.Clamp (Pm.hp + Heal_points, 0, Pm.maxHP);
			delay (1);
			return true;
		}
		return false;
	}
	IEnumerator delay(int seconds){
		yield return new WaitForSeconds (seconds);
	}
}
