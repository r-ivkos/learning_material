using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//.
public class DialogueBox : MonoBehaviour {
	public GameObject dBox;
	public Text dText;
	public Text presstocontinue;
	public Text ans1;
	public Text ans2;
	public Color color;
	[HideInInspector]
	public bool dialogueActive;

	void Start () {
		ans1.enabled = false;
		ans2.enabled = false;
		dText.enabled = true;
		presstocontinue.enabled = true;
		dBox.SetActive (false);
		dialogueActive = false;
	}
		
	public void ShowText (string dialogue){
		dBox.SetActive (true);
		dialogueActive = true;
		dText.text = dialogue;
		ans1.enabled = false;
		ans2.enabled = false;
		dText.enabled = true;
		presstocontinue.enabled = true;

	}
	public void HideText(){
		dBox.SetActive (false);
		dialogueActive = false;
		Time.timeScale = 1f;
	}
	public void ans1Orans2(string first_answer, string second_answer, int answer_selected){
		ans1.enabled = true;
		ans2.enabled = true;
		dText.enabled = false;
		presstocontinue.enabled = false;
		string colorHex = ColorUtility.ToHtmlStringRGB (color);
		if (answer_selected == 1) {
			ans1.text = "<color=#" + colorHex + ">" + first_answer + "</color>";
			ans2.text = second_answer;
		} 
		else if (answer_selected == 2) {
			ans1.text = first_answer;
			ans2.text = "<color=#" + colorHex + ">" + second_answer + "</color>";
		}
		
	
	}
}
