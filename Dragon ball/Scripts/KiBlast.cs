using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiBlast : MonoBehaviour {


	public float speed;
	public float damage;
	public float KiCost;
	[HideInInspector]
	public float direction;
	[HideInInspector]
	public int player;

	void Update () {
		AnimatorStateInfo si = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
		GetComponent<Animator> ().SetFloat ("direction", direction);
		if (si.IsName("Ki fly")){
			//rb.MovePosition (rb.position + new Vector2 (direction * speed * Time.deltaTime, 0));
			transform.position += new Vector3(Time.deltaTime*speed*direction, 0, 0);
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag != "Blast" && col.tag != "Player" + player && col.tag != "Enemy") {
			if(col.tag.Contains("Player")){
				col.gameObject.transform.parent.gameObject.GetComponent<player_movement>().Hit(damage);
			}
			Destroy (gameObject);
		} 
	}
}
