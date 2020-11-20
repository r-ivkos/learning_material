using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {


	private Inventory inv;
	public int objectID;

	void Start(){
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
	}
	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "Player") {
			if (Input.GetKeyDown (KeyCode.Space)) {
				inv.AddObject (objectID);
				if(inv.objects_number != 13)
				Destroy (gameObject);
			}
		}
	}
	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "Player") {
		}
	}
}
