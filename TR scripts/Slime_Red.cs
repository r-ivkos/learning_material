using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Red : MonoBehaviour {


	public float Speed;
	public float Range;
	public int Damage;
	private int Npos;
	private bool pause;

	private GameObject player;
	private Vector3 initialPosition;
	private Vector3 Position1;
	private Vector3 Position2;
	private Vector3 Position3;
	private Vector3 Position4;
	private Animator anim;
	private Rigidbody2D rb;
	private Depth depthFix;
	private SoundController sound; 

	public int MaxHp;
	public int HP;
	private SpriteRenderer sr; 
	bool dying = false;
	private Vector3 target;
	public bool attacked;
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		sound = GameObject.Find ("Sound Controller").GetComponent<SoundController> ();
		depthFix = GetComponent<Depth> ();
		initialPosition = transform.position;
		Position1 = new Vector3 (-7.6f , -60.69f,0);
		Position2 = new Vector3 (-4.15f , -56.1f,0);
		Position3 = new Vector3 (5.21f,-50.5f,0);
		Position4 = new Vector3 (0.27f , -61.08f,0);
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D> ();
		HP = MaxHp;
		sr = GetComponent<SpriteRenderer> ();
		Npos = 0;
	}

	void Update () {
		if (!pause && !attacked) {
			
			if (Npos == 0) {
				target = initialPosition;

				if (transform.position == initialPosition) {
					StartCoroutine (Pause ());
					Npos++;

				}
			} else if (Npos == 1) {
				
				target = Position1;
				if (transform.position == Position1) {
					StartCoroutine (Pause ());
					Npos++;

				}
			} else if (Npos == 2) {
				
				target = Position2;

				if (transform.position == Position2) {
					StartCoroutine (Pause ());
					Npos++;

				}
			} else if (Npos == 3) {
				
				target = Position3;

				if (transform.position == Position3) {
					StartCoroutine (Pause ());
					Npos++;
				
				}
			} else if (Npos == 4) {
				target = Position4;
				if (transform.position == Position4) {
					StartCoroutine (Pause ());
					Npos = 0;
				
				}
			}
		}



		if (!dying && !attacked) {
			//target = initialPosition;
			float distance = Vector3.Distance (player.transform.position, transform.position);

			if (distance < Range) {
				target = player.transform.position;
				depthFix.fixEveryFrame = true;
			
			
			} else
				depthFix.fixEveryFrame = false;
			transform.position = Vector3.MoveTowards (transform.position, target, Speed * Time.deltaTime);
				

			if (rb.velocity != Vector2.zero && !attacked) {
				rb.velocity = Vector2.zero;
			} 
		}
	}


	void Attacked(int damage) 	{
		sound.PlaySound ("Slime");
		HP -= damage;
		if (HP <= 0) {
			anim.SetTrigger ("REDSLIMEDIE");
			dying = true;
			StartCoroutine (Destroy ());
		}

		if (!dying) {
			sr.color = Color.red;
			StartCoroutine (Recover ());
		}
	}
	IEnumerator Recover(){
		yield return new WaitForSeconds (0.2f);
		sr.color = Color.white;
	}
	IEnumerator Destroy(){
		Destroy (rb);
		yield return new WaitForSeconds (0.3f);
		Destroy (gameObject);
		player.GetComponent<Player_Movement> ().attackedDelay = 0;
	}
	IEnumerator Pause(){
		pause = true;
		yield return new WaitForSeconds (3f);
		pause = false;
	}


	void OnDrawGizmos (){
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (transform.position, Range);
	}
	public void Attack(){
		StartCoroutine(player.GetComponent<Player_Movement> ().Attacked(Damage, dying));
	}
	public void KnockBack(KnockBackSettings kbs){
		if (!dying) {
			attacked = true;
			if(rb != null)rb.AddForce (kbs.Direction * kbs.Force);
			StartCoroutine (KnockBackControl (kbs.Time));
		}
	}

	IEnumerator KnockBackControl (float time){
		yield return new WaitForSeconds (time);
		if(rb!=null)rb.velocity = Vector2.zero;
		attacked = false;
	}

}
