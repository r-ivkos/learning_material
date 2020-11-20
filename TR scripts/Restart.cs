using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

	public Text restartText;
	bool restart;

	void Start () {
		restartText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!restart) {
			StartCoroutine (Delay ());
		}
		else  {
			if (Input.GetKey (KeyCode.Space)) {
				SceneManager.LoadScene ("Game");
			}
		
		}

	}

	IEnumerator Delay(){
		yield return new WaitForSeconds (2);
		restartText.enabled = true;
		restart = true;
	
	}
}
