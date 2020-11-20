using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//.
public class Learn_Spell : MonoBehaviour {
	[Tooltip("Dash, Slash, FireBall")]
	public string skill; 
	Animator anim;
	BoxCollider2D bcol;
	Player_Movement pm;
	int SpaceBug = 0;
	int UnlockTextManage = 0;
	string[] UnlockText;
	DialogueBox dbox;

	void Awake () {
		anim = GetComponent<Animator> ();
		bcol = GetComponent<BoxCollider2D> ();
		pm = FindObjectOfType<Player_Movement> ();
		dbox = FindObjectOfType<DialogueBox> ();
	}

	void Update(){
		if (UnlockTextManage > 0){
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (UnlockTextManage >= UnlockText.Length) {
					dbox.HideText ();
					pm.SafeZone = false;
					pm.dialogueActive = false;
					Destroy (gameObject);
				} 
				else {
					dbox.ShowText (UnlockText [UnlockTextManage]);
					UnlockTextManage++;
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			anim.SetBool ("Open", true);
			bcol.size = new Vector2 (1.1f, 0.78f);
			bcol.offset = new Vector2 (-0.47f, 0.37f);
			pm.SafeZone = true;
			}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player"){
			if (Input.GetKeyUp (KeyCode.Space) && SpaceBug == 0)
				StartCoroutine (Learn ());
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			anim.SetBool ("Open", false);
			bcol.size = new Vector2(0.7f, 0.79f);
			bcol.offset = new Vector2 (-0.3f, 0.4f);
			pm.SafeZone = false;
		}
	}

	IEnumerator Learn(){
		
		SpaceBug = 1;
		pm.dialogueActive = true;
		Animator aura = transform.GetChild (0).GetComponent<Animator> ();
		aura.Play ("SpellBook_PickUp");
		yield return new WaitForSeconds (3f);
		Destroy (transform.GetChild (0).gameObject);
		anim.speed = 0.5f;
		anim.SetBool ("Open", false);
		yield return new WaitForSeconds (0.571f * 2);
		GetComponent<SpriteRenderer> ().enabled = false;
		Time.timeScale = 0;
		switch (skill) {
		case "Dash":
			pm.Skills [0] = true;
			UnlockTextManage = 1;
			UnlockText = new string[2] {"Felicidades! Has desbloqueado \"<color=brown>Dash</color>\"!", "Pulsa Z para hacer un dash"};
			dbox.ShowText (UnlockText [0]);
			break;
		case "Slash":
			pm.Skills [1] = true;
			UnlockTextManage = 1;
			UnlockText = new string[2] {"Felicidades! Has desbloqueado \"<color=brown>Cuchillada</color>\"!", "Manten C para lanzar un rayo con espada"};
			dbox.ShowText (UnlockText [0]);
			break;
		case "FireBall":
			pm.Skills [2] = true;
			UnlockTextManage = 1;
			UnlockText = new string[2] {"Felicidades! Has desbloqueado \"<color=brown>Bola de Fuego</color>\"!","Manten X para lanzar la bola de fuego"};
			dbox.ShowText (UnlockText [0]);
			break;
		default:
			print ("oye, no le has asignado habilidad que desbloquar!");
			break;
		}

	}
		
}
