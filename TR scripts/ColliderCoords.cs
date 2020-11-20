using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCoords : MonoBehaviour {

	// Use this for initialization
	void OnCollisionStay2D(Collision2D col){
		if (col.collider.tag == "Enemy") {
			foreach (ContactPoint2D cp in col.contacts) {
				print (cp.normal);
			}
		}
	}
}
