using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THE_LOG : MonoBehaviour {


	public float Speed;
	public float Range;
	public int Damage;

	private GameObject player;
	private Vector3 initialPosition;
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
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D> ();
		HP = MaxHp;
		sr = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		if (!dying && !attacked) {
			target = initialPosition;
			float distance = Vector3.Distance (player.transform.position, transform.position);
			if (transform.position == initialPosition)
				anim.SetBool ("THELOGMOVING", false);
			else
				anim.SetBool ("THELOGMOVING", true);
		
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
		sound.PlaySound ("THE LOG");
		HP -= damage;
		if (HP <= 0) {
			anim.SetTrigger ("THELOGDIE");
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
		Destroy (GetComponent<BoxCollider2D> ());
		Destroy (rb);
		yield return new WaitForSeconds (0.3f);
		Destroy (gameObject);
		player.GetComponent <Player_Movement> ().attackedDelay = 0;
		player.GetComponent<Player_Movement> ().Kill ();


	
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
			if(rb!=null)rb.AddForce (kbs.Direction * kbs.Force);
			StartCoroutine (KnockBackControl (kbs.Time));
		}
	}

	IEnumerator KnockBackControl (float time){
		yield return new WaitForSeconds (time);
		if (rb != null)rb.velocity = Vector2.zero;
		attacked = false;
	}

}
