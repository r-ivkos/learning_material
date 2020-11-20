using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {
	public float Delay;
	public int damage;
	[HideInInspector ]
	public Vector2 movement;
	public float speed;

	void Update () {

		transform.position += new Vector3 (movement.x, movement.y, 0) * speed * Time.deltaTime;
	}

	IEnumerator OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Object") {
			yield return new WaitForSeconds (Delay);
			Destroy (gameObject);
		} else if (other.tag != "Player" && other.tag != "Sword" && other.tag != "NPC") {

			if (other.tag == "Enemy") other.SendMessage ("Attacked", damage);
			Destroy (gameObject);
		}
	}
}