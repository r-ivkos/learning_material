using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public float KnockBackForce;
	public float KnockBackTime;
	void OnTriggerEnter2D (Collider2D Other) {
		if (Other.tag == "Enemy") Other.SendMessage ("Attacked", 1);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.collider.tag == "Enemy") {
			foreach (ContactPoint2D cp in col.contacts) {
				Vector2 knockback = -cp.normal;
				KnockBackSettings kbs = new KnockBackSettings (KnockBackForce, KnockBackTime, knockback);
				col.collider.SendMessage ("KnockBack", kbs);
			}
		}
	}
}


public struct KnockBackSettings{
	public float Force;
	public float Time;
	public Vector2 Direction;

	public KnockBackSettings(float Force, float Time, Vector2 Direction){
		this.Force = Force;
		this.Time = Time;
		this.Direction = Direction;
	}
}
