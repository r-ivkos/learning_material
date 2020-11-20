using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCombo : MonoBehaviour{
	public KeyCode[] combo;
	public float delayTime, coolDownTime;
	[HideInInspector]
	public int key_index;
	private bool coolDown;

	public KeyCode GetKeyAt(int index){
		return combo[index];
	}
	public bool DetectCombo(KeyCode keyPressed = KeyCode.None){
		if (((Input.GetKeyDown(combo[key_index]) && keyPressed == KeyCode.None) || (keyPressed == combo[key_index])) && !coolDown) {
			key_index++;
			if (key_index > 1){
				StopAllCoroutines ();
			}
		    StartCoroutine (comboTimer ());
			if (key_index >= combo.Length) {
				StopAllCoroutines();
				key_index = 0;
				StartCoroutine(CoolDown());
				return true;
			}
		}

		else if (keyPressed != KeyCode.None && keyPressed != combo[key_index] && !coolDown){
			key_index = 0;
		}

		return false; 
	}
	IEnumerator comboTimer (){
		yield return new WaitForSeconds (delayTime);
		key_index = 0;
	}

	IEnumerator CoolDown(){
		coolDown = true;
		yield return new WaitForSeconds(coolDownTime);
		coolDown = false;
	}
}
