using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_movement : MonoBehaviour {

	//KEYS
	public KeyCode Right, Left, Jump, Down, KiCharge, KiAttack, Block;


	public float speed, speed_fast, jump_force, health, MaxKi;
	public int MaxKiCombo;
	public GameObject ki_blast;
	public GameObject rival;
	public StatsManager sm;
	private player_movement rivalScript;
	[HideInInspector]
	public float damage;
	[HideInInspector]
	public bool critical, ganador, attack;
	private float MaxHp, Ki;
	private int lastDir, kiCombo;
	private bool kiCharge, dashing, kiAttack, invins, hited;
	private KeyCode keyPressed;
	Animator anim;
	Canvas winWindow;
	Text winText; 
	public KeyCombo[] combo;
	Rigidbody2D rb;
	Vector2 movement;
	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		rivalScript = rival.GetComponent<player_movement>();
		lastDir = 1;
		MaxHp = health;
		kiCombo = 0;
		Ki = 0;
		winWindow = GameObject.Find("Win!").GetComponent<Canvas>();
		winWindow.enabled = false;
		winText = winWindow.gameObject.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo si = anim.GetCurrentAnimatorStateInfo (0);
		if(health > 0 && !ganador){
			anim.SetBool ("Moving", false);
			anim.SetBool ("Dash", dashing);
			kiCharge = anim.GetBool("Ki_charge");

			invins = (anim.GetBool("MegaHit") || anim.GetBool("Block"));
			hited = si.IsName("Hit");
	        kiAttack = (si.IsName ("Ki_launch_1") || si.IsName ("Ki_launch_2"));
			if(anim.GetBool("Jump") == false && !attack && !kiAttack && !invins && !kiCharge) {
				int dir = 0;
				if(Input.GetKey(Right)) dir = 1;
				if(Input.GetKey(Left)) dir = -1;
				if(Input.GetKey(Right) && Input.GetKey(Left)) dir = 0;
				movement = new Vector2 (dir, 0);
				if(transform.position.x < rival.transform.position.x) {
					transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
				}
				else {
					transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
				}

			}
			if (movement != Vector2.zero) { 
				anim.SetBool ("Moving", true);
				transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * movement.x, transform.localScale.y, 0);
			} 
			else {
				dashing = false;
			}
			if (anim.GetBool("Jump") == false && !attack && !kiAttack && !hited){
				if(Input.GetKey(KiCharge) &&  !invins && Ki != MaxKi) {
					anim.SetBool ("Ki_charge", true);
					Ki = Mathf.Clamp(Ki+0.5f, 0, MaxKi);
				}
				else anim.SetBool ("Ki_charge", false);
				if(Input.GetKey(Block) &&  !kiCharge) anim.SetBool("Block", true);
				else anim.SetBool("Block", false);
			}
			if (Input.GetKeyDown (KiAttack) && anim.GetBool ("Ki_charge") == false  &&  !invins && !attack && kiCombo < MaxKiCombo && Ki - ki_blast.GetComponent<KiBlast>().KiCost >= 0 && !hited) {
				if(kiAttack) kiCombo++;
				else kiCombo = 0;
				anim.SetTrigger ("Ki attack");
			}
			if (kiCombo >= MaxKiCombo && kiCombo < 100){
				StartCoroutine(RelaxMan());
			}
			if (anim.GetBool ("Ki_charge") == false && anim.GetBool("Jump") == false && !invins && !hited) {
				if (Input.GetKeyDown (Jump) && !kiAttack  && !attack) {
					lastDir = (int)(movement.x);
					anim.SetBool ("Jump", true);
					rb.AddForce (new Vector2 (0, jump_force), ForceMode2D.Impulse);
				}
				if(Input.GetKey(Down) && Input.GetKey(KiAttack) && !attack && !kiAttack){
					anim.SetTrigger("Kame");
				}
				if (Input.anyKeyDown) {
					bool completed = combo [0].DetectCombo (keyPressed);
					if(combo[0].key_index == 1)
					{
						anim.SetTrigger ("Attack");
					}
					if (combo[0].key_index==2){
						anim.SetTrigger("ElbowHit");
					}
					if (completed){
						anim.SetTrigger("Hook");
					}
					completed = combo[3].DetectCombo(keyPressed);
					if (combo[3].key_index == 1) {
						anim.SetTrigger ("Kick");
					}
					if (combo[3].key_index == 2) {
						anim.SetTrigger ("TripleKick");
					}
					if (combo[3].key_index == 3) {
						anim.SetTrigger ("Kick2");
					}
					if (completed) {
						anim.SetTrigger ("BackflipKick");
					}
				}
			}

			if(anim.GetBool("Jump")){
				if(!attack){
					if (Input.GetKey(combo[0].GetKeyAt(0))) anim.SetTrigger("airPunch");
					if (Input.GetKey(combo[3].GetKeyAt(0))) anim.SetTrigger("airKick");
				}

				if(kiAttack || attack) {
					if(kiAttack)rb.velocity = Vector2.zero;
					if(attack) rb.velocity = new Vector2(rb.velocity.x, -5);
					lastDir = 0;
				}
				switch((int)anim.GetFloat("jump_state")){

				case 1:
					
					if (rb.velocity.y <= 0) {
						anim.SetFloat ("jump_state", 2);
					}
					break;
				case 2:
					
					if (rb.velocity == Vector2.zero && !kiAttack && !attack) {
						
						anim.SetFloat ("jump_state", 3);
						anim.Play ("Jump", 0, 0);
					}
					break;
				case 3:
					if (si.normalizedTime > 1 || kiAttack || attack) {
						anim.SetFloat ("jump_state", 1);
						anim.SetBool ("Jump", false);
						movement = Vector2.zero;
					}
					break;
				}
			}

			if (combo [1].DetectCombo () || combo [2].DetectCombo ()) {
				dashing = true;
			}
			sm.Manage(health, MaxHp, Ki, MaxKi);
		}
		else if(!ganador){
			if(!anim.GetBool("MegaHit")){
				float rDir = rival.transform.localScale.x/Mathf.Abs(rival.transform.localScale.x);
				rb.AddForce(new Vector2(rDir*5, 20), ForceMode2D.Impulse);
			}
			anim.SetBool("MegaHit",true);
			anim.SetTrigger("SuperHit");
			rivalScript.ganador = true;
		}
		else if(ganador){
			//print(tag + " HA GANADO!");
			if(!si.IsName("Win"))anim.SetTrigger("Winner");
			winWindow.enabled = true;
			switch(tag){
			case "Player1":
				winText.text = "Player 1 ha ganado!";
				break;
			case "Player2":
				winText.text = "Player 2 ha ganado!";
				break;
			}

			if(Input.GetKeyDown(KeyCode.Return)){
				SceneManager.LoadScene("Game");
			}
		}
		if(anim.GetBool("MegaHit")){ 

			switch((int)anim.GetFloat("fall_state")){

			case 1:
				if (rb.velocity.y <= 0) {
					anim.SetFloat ("fall_state", 2);
				}
				break;
			case 2:
				if (rb.velocity.y == 0) {
					anim.SetFloat ("fall_state", 3);
				}
				break;
			case 3:
				if ((Input.GetKeyDown(Right) || Input.GetKeyDown(Left) || Input.GetKeyDown(Jump)) && health > 0) {
					anim.SetFloat ("fall_state", 4);
					anim.Play ("Fall", 0, 0);
				}
				break;
			case 4:
				if(si.normalizedTime > 1){
					anim.SetBool ("MegaHit", false);
					anim.SetFloat("fall_state", 1);
				}
				break;
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
	void FixedUpdate(){
		float movSpeed = speed;
		if (dashing) movSpeed = speed_fast;
		if (!kiCharge && !attack && !kiAttack && !invins && !hited) {
			if (anim.GetBool ("Jump") == false ) rb.position = Vector2.MoveTowards (rb.position, rb.position + movement, Time.deltaTime * movSpeed);
			else if (anim.GetFloat ("jump_state") < 3) rb.position = Vector2.MoveTowards (rb.position, rb.position + new Vector2 (lastDir, 0), Time.deltaTime * movSpeed);
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag.Contains("Player")) {
			if(critical) {
				int dir = (int)(transform.localScale.x/Mathf.Abs(transform.localScale.x));
				if(!rivalScript.invins)rivalScript.KnockBack(new Vector2(2*dir, 10));
			}
			rivalScript.Hit(damage, critical);
		}
	}

	void OnGUI() {
		Event key = Event.current;
		if (key.isKey) {
			if (key.keyCode != KeyCode.None) {
				keyPressed = key.keyCode;
			}

		}
	}

	public void Ki_Blast(){
		KiBlast kb = Instantiate (ki_blast).GetComponent<KiBlast> ();
		kb.direction = transform.localScale.x/Mathf.Abs(transform.localScale.x);
		kb.gameObject.transform.position = transform.position + new Vector3 (2.3f*kb.direction, -0.75f, -1);
		switch(tag){
		case "Player1": kb.player = 1; break;
		case "Player2": kb.player = 2; break;
		}
		Ki -= kb.KiCost;
	}
	IEnumerator RelaxMan(){
		kiCombo = 100;
		yield return new WaitForSeconds(1.5f);
		kiCombo = 0;
	}

	public void Hit (float damage, bool fatalHit = false){
		if(!fatalHit && anim.GetBool("Jump") == false && !invins) anim.SetBool("Hit", true);
		else if(!invins) {
			anim.SetBool("MegaHit", true);
			anim.SetTrigger("SuperHit");
			anim.SetBool("Jump", false);
		}
		if(!invins) health-=damage;
	}

	public void KnockBack(Vector2 force){
		rb.AddForce(force, ForceMode2D.Impulse);
	}
	public void MoveFromAnimation(int mov){
		Vector2 force = Vector2.zero;
		float dir = transform.localScale.x/Mathf.Abs(transform.localScale.x);
		switch(mov){
		case 1:
		case 2: 
			force = new Vector2(10*mov, 0);
			break;
		case 3:
			force = new Vector2(15, 0);
			break;
		case 4:
			force = new Vector2(5, 20);
			break;
		case 5:
			force = new Vector2(10, 5);
			break;
		}
		force.x = dir*force.x;
		rb.AddForce(force, ForceMode2D.Impulse);
	}
	public void StopFromAnimation(){
		rb.velocity = Vector2.zero;
	}
}

