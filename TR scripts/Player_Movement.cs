
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour{
	[Tooltip("Todas las habilidades desbloqueadas")]
	public bool GODMODE;
	public float moveSpeed; 
	public float DashSpeed;
	public float DashTime;
	public float maxHP;
	public float invincibilityTime;
	public GameObject Camera;
	public GameObject initialMap;
	public GameObject Slash;
	public GameObject Fireball;


	private Animator anim;
	private Animator cameraAnim;
	private Rigidbody2D rb2d;
	private Vector2 movement;
	private SpriteRenderer sr;
	private SoundController sound;
	[HideInInspector]
	public float hp;
	private bool inmortality;
	private Aura aura;
	private FireBall fireball;
	private GameObject fball;
	private HealthBar hub;
	private CircleCollider2D[] attack;
	[HideInInspector] 
	public float dashTime; 
	[HideInInspector]
	public int DashDirection, kills_thelog, kills_slime,
	missionsActive, missions_thelog, missions_slime = 0;
	[HideInInspector]
	public bool SafeZone, dialogueActive;
	[HideInInspector]
	public bool[] Skills;
	[HideInInspector]
	public bool[] SelectedSkill;
	[HideInInspector]
	public bool Slashing, FireBallCharging, Dashing,  
	kill_mission_thelog, kill_mission_slime;
	int i; 

	void Start () { 
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		rb2d = GetComponent<Rigidbody2D> ();
		cameraAnim = Camera.GetComponent<Animator> ();
		hub = FindObjectOfType<HealthBar> ();
		dashTime = DashTime;
		GameObject.Find("Kamera").GetComponent<MainCamera>().BORDES(initialMap);
		sound = GameObject.Find ("Sound Controller").GetComponent<SoundController> ();
		attack = transform.GetChild (0).GetComponents<CircleCollider2D> ();
		attack[0].enabled = false;
		attack [1].enabled = false;
		hp = maxHP;	
		aura = transform.GetChild (1).GetComponent<Aura>();
		if (!GODMODE)
		Skills = new bool[3] {false, false, false}; // 0 - Dash, 1 - Slash, 2 - FireBall
		else 
		Skills = new bool[3] {true, true, true}; // 0 - Dash, 1 - Slash, 2 - FireBall
		SelectedSkill = new bool[2] {false, false}; // 0 - Slash, 1 - FireBall
		Slashing = false;
		FireBallCharging = false;
		Dashing = false;
	}


	void Update () {
		AnimatorStateInfo si = anim.GetCurrentAnimatorStateInfo (0);
		bool attacking = si.IsName ("Attack");
		movement = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		anim.SetBool ("PlayerMoving", false);
		anim.SetBool ("CancelCharge", false);
		anim.SetBool ("FireBallRelease", false);

		if (movement != Vector2.zero && (!Dashing || !Skills [0]) && !dialogueActive && !FireBallCharging) {
			if (!Slashing)
				anim.SetBool ("PlayerMoving", true);
			if ((movement.x == 1 || movement.x == -1) && (movement.y == 1 || movement.y == -1)) {
				if (movement.x == 1)
					i = 1;
				else if (movement.x == -1)
					i = -1;
				anim.SetFloat ("MoveX", 0);
			} else {
				i = 0;
				anim.SetFloat ("MoveX", movement.x);
			}
			anim.SetFloat ("MoveY", movement.y);

		}

		if (Input.GetKeyDown ("space") && !attacking && !SafeZone && !dialogueActive && !FireBallCharging) {
			anim.SetTrigger ("PlayerAttack");
			rb2d.MovePosition (rb2d.position);
			sound.PlaySound ("Attack");
			inmortality = true;
		} else if (!attacking) {
			inmortality = false;
		}
		if (movement != Vector2.zero) {
			if (movement == Vector2.down)
				attack [0].offset = new Vector2 (movement.x / 2.5f, -0.5f);
			else
				attack [0].offset = new Vector2 (movement.x / 2f, movement.y / 2f);
			if (movement == Vector2.up) {
				attack [1].offset = attack [0].offset + Vector2.down * 0.5f;
			} 
			else if (movement == Vector2.left) {
				attack [1].offset = attack [0].offset + new Vector2 (0.2f, -0.1f);
			}
			else if (movement == Vector2.right) {
				attack [1].offset = attack [0].offset + new Vector2 (-0.2f, -0.1f);
			}
				else attack [1].offset = attack [0].offset + Vector2.down * 0.1f;
		}
		if (attacking) {
			float playbackTime = si.normalizedTime;
			if (playbackTime > 0.3166f && playbackTime < 0.6333f) {
				attack [0].enabled = true;
				attack [1].enabled = true;

			} 
			else {
				attack [0].enabled = false;
				attack [1].enabled = false;

			}
		}
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		bool charging = stateInfo.IsName ("Slash");
		bool fballCharging = stateInfo.IsName ("FireBall");

		if (Input.GetKeyDown (KeyCode.C)) {
			if (SelectedSkill [0] && Skills [1] && !FireBallCharging && !dialogueActive && !Dashing) { 
				anim.SetTrigger ("Charging");
				aura.AuraStart ();
			} else if (SelectedSkill [1] && Skills [2] && !Slashing && !dialogueActive && !Dashing) {
				FireBallCharging = true;
				anim.SetTrigger ("FireBallCharging");
				fball = Instantiate (Fireball, transform.position + Vector3.up * 1.6f, Quaternion.identity);
			}
				
		} else if (Input.GetKeyUp (KeyCode.C)){
			if (SelectedSkill [0] && Skills [1] && charging && !FireBallCharging && !dialogueActive && !Dashing) {
				anim.SetBool ("CancelCharge", true);  
				if (aura.IsCharged ()) { 
					anim.SetTrigger ("PlayerAttack");
					float angle = 0;
					if (i == 0)
						angle = Mathf.Atan2 (anim.GetFloat ("MoveY"), anim.GetFloat ("MoveX")) * Mathf.Rad2Deg;
					else if (i == -1)
						angle = Mathf.Atan2 (anim.GetFloat ("MoveY"), -1f) * Mathf.Rad2Deg;
					else if (i == 1)
						angle = Mathf.Atan2 (anim.GetFloat ("MoveY"), 1f) * Mathf.Rad2Deg;
				
					GameObject slashObj = Instantiate (Slash, transform.position, Quaternion.AngleAxis (angle, Vector3.forward));
					Slash slash = slashObj.GetComponent<Slash> ();
					if (i == 0)
						slash.movement.x = anim.GetFloat ("MoveX");
					else if (i == 1)
						slash.movement.x = 1f;
					else if (i == -1)
						slash.movement.x = -1f;
					slash.movement.y = anim.GetFloat ("MoveY");
				}
				aura.AuraStop ();
				StartCoroutine (Recover (0.5f));

			} else if (SelectedSkill[1] && Skills [2] && !Slashing && !dialogueActive && !Dashing && fballCharging) {
				fireball = fball.GetComponent<FireBall> ();
				if (fireball.charged) {
					fireball.movement.x = anim.GetFloat ("MoveX");
					fireball.movement.y = anim.GetFloat ("MoveY");
					anim.SetBool ("FireBallRelease", true);
					fireball.release = true;
					StartCoroutine (Recover (0.5f));
				} 
				else {
					Destroy (fball);
					anim.SetBool ("CancelCharge", true);
					FireBallCharging = false;
				}
			}
	}
			if (charging) {
				Slashing = true;
			}
			
		if (Input.GetKey (KeyCode.C) && SelectedSkill[1] && Skills [2] && !Slashing && !dialogueActive && !Dashing && !FireBallCharging && fballCharging) {
			FireBallCharging = true;
		}


	}


	IEnumerator Recover (float seconds) {
		yield return new WaitForSeconds(seconds);
		Slashing = false;
		FireBallCharging = false;
		}


	void FixedUpdate(){
		
		Vector2 fixedMovement = movement;
		if (fixedMovement.x == fixedMovement.y || fixedMovement.x == -fixedMovement.y || -fixedMovement.x == fixedMovement.y)
			fixedMovement = new Vector2 (Mathf.Sqrt (0.5f) * fixedMovement.x, Mathf.Sqrt (0.5f) * fixedMovement.y);
		if (movement != Vector2.zero && (!Dashing || !Skills[0]) && !anim.GetBool ("PlayerDashing") && !Slashing && !dialogueActive && !FireBallCharging) {
			rb2d.MovePosition (rb2d.position + fixedMovement * moveSpeed * Time.deltaTime);
		}


			

		DashSkill ();

	}

	[HideInInspector]
	public int attackedDelay = 0;
	void OnCollisionStay2D(Collision2D Collision){
		if (Collision.collider.tag == "Enemy" && !inmortality) {
			if (attackedDelay == 0) {
				Collision.gameObject.SendMessage ("Attack");
				attackedDelay = 1;
			}
		}
	}
		

	public IEnumerator Attacked(int damage, bool enemeyDying){
		if (!enemeyDying) {
			hub.DamageBar (damage);
			hp = hp - damage;
			bool dying = false;
			if (hp <= 0) {			
				dying = true;

				SceneManager.LoadScene ("Game Over");

			}
			if (!dying) {
				sr.color = Color.red;
				StartCoroutine (Recover ());
			}
		}
		yield return new WaitForSeconds (invincibilityTime);
		attackedDelay = 0;

	}
		
	IEnumerator Recover(){
		yield return new WaitForSeconds (0.2f);
		sr.color = Color.white;
	}



	public void Kill(){
		if (missionsActive > 0) {
			if (kill_mission_thelog) {
				kills_thelog++;
			}
			if (kill_mission_slime) {
				kills_slime++;
			}
		}
	}

	public void ResetKill(int Kills_To_Rest = 1){
		if (missionsActive > 1) {
			missionsActive--;
			if (kill_mission_thelog) {
				kills_thelog -= Kills_To_Rest;
			} else if (kill_mission_slime) {
				kills_slime -= Kills_To_Rest;
			} else if (kill_mission_slime && kill_mission_thelog) {
				kills_slime = 0;
				kills_thelog = 0;
			}
		}
		else if (missionsActive == 1) {
			missionsActive--;
			kills_slime = 0;
			kills_thelog = 0;
		}

		if (kills_thelog == 0 && missions_thelog == 0) {
			kill_mission_thelog = false;
		}
		if (kills_slime == 0 && missions_slime == 0) {
			kill_mission_slime = false;
		}
	}

	void DashSkill (){
		if(!dialogueActive && !Slashing && Skills[0] && !FireBallCharging){	
			if (DashDirection == 0) {
				if (Input.GetKeyDown (KeyCode.Z)) {
					Dashing = true;
					sound.PlaySound ("Dash");
					cameraAnim.SetTrigger ("Dash");
					if (anim.GetFloat ("MoveX") == 1f) {
						DashDirection = 1; // derecha
					} 
					else if (anim.GetFloat ("MoveX") == -1f) {
						DashDirection = 2; //izquierda
					} 						
					else if (anim.GetFloat ("MoveY") == 1f) {
						DashDirection = 3; // arriba
					} 
					else if (anim.GetFloat ("MoveY") == -1f) {
						DashDirection = 4; // abajo
					}
				}

			}	 
			else {
				if (dashTime <= 0) {
					dashTime = DashTime;
					rb2d.velocity = Vector2.zero;
					anim.SetBool ("PlayerDashing", false);
					rb2d.MovePosition (rb2d.position);
					DashDirection = 0;
					Dashing = false;

				} 
				else {
					dashTime -= Time.fixedDeltaTime;
					anim.SetBool ("PlayerDashing", true);
					if (DashDirection == 1) {
						rb2d.velocity = Vector2.right * DashSpeed;
					}	 
					else if (DashDirection == 2) {
						rb2d.velocity = Vector2.left * DashSpeed;
					}
					else if (DashDirection == 3) {
						rb2d.velocity = Vector2.up * DashSpeed;
					}
					else if (DashDirection == 4) {
						rb2d.velocity = Vector2.down * DashSpeed;
					}
				}
			}
		}
	}


}