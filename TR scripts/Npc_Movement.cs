using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Movement : MonoBehaviour {

	public float moveSpeed;
	public float tiempo;
	private Mikel LOL;
	int KK = 0;
	int KK2 = 0;
	private Vector3 movementUp;
	private Vector3 movementDown;
	private Vector3 movementLeft;
	private Vector3 movementRight;
	private Animator anim;


	private int WalkDirection;

	void Start () {
		anim = GetComponent<Animator> ();
		LOL = GetComponent <Mikel> ();
		LOL.Dogo = false;
		movementUp = new Vector3 (0, 1, 0);
		movementLeft = new Vector3 (-1, 0, 0);
		movementRight = new Vector3 (1, 0, 0);
		movementDown = new Vector3 (0, -1, 0);
		LOL.Dogo = false;
	}

	IEnumerator Delay(){
		yield return new WaitForSeconds (tiempo);
		switch (KK2){
		case 0:
			KK++;
			break;
		case 1:
			KK++; 
			break;
		case 2: 
			KK++; 
			break;
		case 3:
			KK++;
			break;
		}
		if (KK == 4)
			KK = 0;	
	}

	void Update () {
		if (LOL.Dogo == true) {
			anim.SetBool ("Entering", true);

		} else {
			anim.SetBool ("Entering", false);
				if (KK == 0) {
					transform.position = Vector3.MoveTowards (transform.position, transform.position + movementUp, moveSpeed * Time.deltaTime);
					anim.SetFloat ("MoveY", 1f);
					anim.SetFloat ("MoveX", 0f);
					if (KK2 == 0) {
						StartCoroutine (Delay ());
						KK2 = 1;
					}
				}

				if (KK == 1) {
					transform.position = Vector3.MoveTowards (transform.position, transform.position + movementLeft, moveSpeed * Time.deltaTime);
					anim.SetFloat ("MoveX", -1f);
					anim.SetFloat ("MoveY", 0f);
					if (KK2 == 1) {
						StartCoroutine (Delay ());
						KK2 = 2;
					}
				}

				if (KK == 2) {
					transform.position = Vector3.MoveTowards (transform.position, transform.position + movementDown, moveSpeed * Time.deltaTime);
					anim.SetFloat ("MoveY", -1f);
					anim.SetFloat ("MoveX", 0f);
					if (KK2 == 2) {
						StartCoroutine (Delay ());
						KK2 = 3;
					}
				}

				if (KK == 3) {
					transform.position = Vector3.MoveTowards (transform.position, transform.position + movementRight, moveSpeed * Time.deltaTime);
					anim.SetFloat ("MoveX", 1f);
					anim.SetFloat ("MoveY", 0f);

					if (KK2 == 3) {
						StartCoroutine (Delay ());
						KK2 = 0;
					}
				}

			}
		}
}