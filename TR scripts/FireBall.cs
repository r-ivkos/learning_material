using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

	public int damage;
	public float speed;
	public float Delay;
	public float TurnDownTime;

	private Animator anim;
	private GameObject player;
	private CircleCollider2D ccol;

	[HideInInspector]
	public Vector3 movement;
	[HideInInspector]
	public bool charged;
	[HideInInspector]
	public bool release;

	private int fixball;

	void Start () {
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Character");
		ccol = GetComponent<CircleCollider2D> ();
		charged = false;
		release = false;
		fixball = 0;
		ccol.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo si = anim.GetCurrentAnimatorStateInfo (0);
		if (!release) {
			transform.position = player.transform.position + Vector3.up * 1.6f;
		}
		if (si.normalizedTime >= 1) {
			charged = true;
			if (release) {
				if (movement.x != 1 && movement.x != -1) {
					ccol.enabled = true;
					transform.position += new Vector3 (movement.x, movement.y, 0) * speed * Time.deltaTime;
				}
				else {
					if (fixball == 0) {
						transform.position += new Vector3 (movement.x, -0.5f, 0) * speed * Time.deltaTime;
						StartCoroutine (FixBall());
					} else if (fixball == 1) {
						transform.position += new Vector3 (movement.x, -0.5f, 0) * speed * Time.deltaTime;
					} else if (fixball == 2) {
						ccol.enabled = true;
						transform.position += new Vector3 (movement.x, movement.y, 0) * speed * Time.deltaTime;
					}
				}
			}

		}
		
	}

	IEnumerator OnTriggerEnter2D (Collider2D other) {
		if (release) {
			if (other.tag == "Object") {
				yield return new WaitForSeconds (Delay);
				Destroy (gameObject);
			} else if (other.tag != "Player" && other.tag != "Sword" && other.tag != "NPC") {
				if (other.tag == "Enemy") {
					other.SendMessage ("Attacked", damage);
				}
				Destroy (gameObject);
			}
		}
	}
	IEnumerator FixBall (){
		fixball = 1;
		yield return new WaitForSeconds (TurnDownTime);
		fixball = 2;
	}


	void OnBecameInvisible(){
		Destroy (gameObject);
	}
		
}
