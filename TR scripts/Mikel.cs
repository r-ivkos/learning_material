using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mikel : MonoBehaviour {
	public bool question;
	public string[] dialogue;
	public string answer1;
	public string answer2;
	public string[] beforeAnswer1;
	public bool anotherAnswer1;
	public string[] aanswer1;
	public string[] beforeAnswer2;
	public bool anotherAnswer2;
	public string[] aanswer2;
	[HideInInspector]
	public bool Dogo = false;
	public bool WantTextCloud;
	public GameObject TextCloud;
	public Vector3 TextCloudPosition;
	public AudioClip music;
	public AudioClip mapMusic;
	private SoundController sound;
	private DialogueBox dBoxx;
	[HideInInspector]
	public bool missionCleared, missionStarted, talkedAboutMission;
	private GameObject textCloud;
	private int HelpForTheLargeText = -1;
	private Player_Movement pm; 
	private bool answering, answered;
	private int selectanswer = 1;
	private bool specialAnswer, specialMissionText;
	private Missions mission;

	void Awake(){
		dBoxx = FindObjectOfType<DialogueBox> ();
		pm = FindObjectOfType<Player_Movement> ();
		mission = GetComponent<Missions> ();
		sound = GameObject.Find ("Sound Controller").GetComponent<SoundController> ();
	}
	void Update(){
		if (!missionCleared) {

			if (!answering && !answered) {
				if (HelpForTheLargeText >= 0 && HelpForTheLargeText < dialogue.Length) {
					if (Input.GetKeyDown (KeyCode.Space)) {
						dBoxx.ShowText (dialogue [HelpForTheLargeText]);
						HelpForTheLargeText++;
					}
				} else if (HelpForTheLargeText >= dialogue.Length && Input.GetKeyUp (KeyCode.Space)) {
					if (HelpForTheLargeText == dialogue.Length)
						HelpForTheLargeText++;
					else if (Input.GetKeyUp (KeyCode.Space) && !question) {
						dBoxx.HideText ();
						pm.dialogueActive = false;
						HelpForTheLargeText = -1;
						if (music != null) {
							sound.PlayMusic (mapMusic);	
						}

						if (WantTextCloud)
							textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
					} else if (Input.GetKeyUp (KeyCode.Space) && question) {
						dBoxx.ans1Orans2 (answer1, answer2, selectanswer);
						answering = true;
						HelpForTheLargeText = 1;
					}

				}
			} else if (!specialAnswer) {
				if (!answered && answering) {
					dBoxx.ans1Orans2 (answer1, answer2, selectanswer);
					if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
						selectanswer = 2;
					} else if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
						selectanswer = 1;
					} else if (Input.GetKeyDown (KeyCode.Space)) {
						answering = false;
						answered = true;
						if (selectanswer == 1)
							dBoxx.ShowText (beforeAnswer1 [0]);
						else if (selectanswer == 2)
							dBoxx.ShowText (beforeAnswer2 [0]);
					}
				} else if (answered && !answering) {
					if (selectanswer == 1) {
						if (HelpForTheLargeText >= 0 && HelpForTheLargeText < beforeAnswer1.Length) {
							if (Input.GetKeyDown (KeyCode.Space)) {
								dBoxx.ShowText (beforeAnswer1 [HelpForTheLargeText]);
								HelpForTheLargeText++;
							}
						} else if (HelpForTheLargeText >= beforeAnswer1.Length && Input.GetKeyUp (KeyCode.Space)) {
							if (HelpForTheLargeText == beforeAnswer1.Length)
								HelpForTheLargeText++;
							else if (Input.GetKeyUp (KeyCode.Space)) {
								dBoxx.HideText ();
								pm.dialogueActive = false;
								HelpForTheLargeText = -1;
								if (!anotherAnswer1) {
									answering = false;
									answered = false;
								} else if (aanswer1.Length != 0) {
									specialAnswer = true;
									if (mission != null) {
										missionStarted = true;
									}
								} else if (mission != null) {
									missionStarted = true;
								}
								if (music != null) {
									sound.PlayMusic (mapMusic);	
								}
								if (WantTextCloud)
									textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
							} 
						}
					} else if (selectanswer == 2) {
						if (HelpForTheLargeText >= 0 && HelpForTheLargeText < beforeAnswer2.Length) {
							if (Input.GetKeyDown (KeyCode.Space)) {
								dBoxx.ShowText (beforeAnswer2 [HelpForTheLargeText]);
								HelpForTheLargeText++;
							}
						} else if (HelpForTheLargeText >= beforeAnswer2.Length && Input.GetKeyUp (KeyCode.Space)) {
							if (HelpForTheLargeText == beforeAnswer2.Length)
								HelpForTheLargeText++;
							else if (Input.GetKeyUp (KeyCode.Space)) {
								dBoxx.HideText ();
								pm.dialogueActive = false;
								HelpForTheLargeText = -1;
								if (!anotherAnswer2) {
									answering = false;
									answered = false;
								} else if (aanswer2.Length != 0) {
									specialAnswer = true;
								}
								if (music != null) {
									sound.PlayMusic (mapMusic);	
								}
								if (WantTextCloud)
									textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
							} 
						}
					}
				}
			} else {
				if (selectanswer == 1) {
					if (HelpForTheLargeText >= 0 && HelpForTheLargeText < aanswer1.Length) {
						if (Input.GetKeyDown (KeyCode.Space)) {
							dBoxx.ShowText (aanswer1 [HelpForTheLargeText]);
							HelpForTheLargeText++;
						}
					} else if (HelpForTheLargeText >= aanswer1.Length && Input.GetKeyUp (KeyCode.Space)) {
						if (HelpForTheLargeText == aanswer1.Length)
							HelpForTheLargeText++;
						else if (Input.GetKeyUp (KeyCode.Space)) {
							dBoxx.HideText ();
							pm.dialogueActive = false;
							HelpForTheLargeText = -1;
							if (music != null) {
								sound.PlayMusic (mapMusic);	
							}
							if (WantTextCloud)
								textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
						} 
					}
				} else if (selectanswer == 2) {
					if (HelpForTheLargeText >= 0 && HelpForTheLargeText < aanswer2.Length) {
						if (Input.GetKeyDown (KeyCode.Space)) {
							dBoxx.ShowText (aanswer2 [HelpForTheLargeText]);
							HelpForTheLargeText++;
						}
					} else if (HelpForTheLargeText >= aanswer2.Length && Input.GetKeyUp (KeyCode.Space)) {
						if (HelpForTheLargeText == aanswer2.Length)
							HelpForTheLargeText++;
						else if (Input.GetKeyUp (KeyCode.Space)) {
							dBoxx.HideText ();
							pm.dialogueActive = false;
							HelpForTheLargeText = -1;
							if (music != null) {
								sound.PlayMusic (mapMusic);	
							}
							if (WantTextCloud)
								textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
						} 
					}
				}
			
			}
		} 
		else {
			if (!specialMissionText) {
				if (HelpForTheLargeText >= 0 && HelpForTheLargeText < mission.Mission_Completed_Text.Length) {
					if (Input.GetKeyDown (KeyCode.Space)) {
						mission.GiveObject ();
						dBoxx.ShowText (mission.Mission_Completed_Text [HelpForTheLargeText]);
						HelpForTheLargeText++;
					}
				} else if (HelpForTheLargeText >= mission.Mission_Completed_Text.Length && Input.GetKeyUp (KeyCode.Space)) {
					if (HelpForTheLargeText == mission.Mission_Completed_Text.Length)
						HelpForTheLargeText++;
					else if (Input.GetKeyUp (KeyCode.Space)) {
						dBoxx.HideText ();
						pm.dialogueActive = false;
						HelpForTheLargeText = -1;
						mission.GiveReward ();
						if (mission.before_mission_text.Length != 0) {
							specialMissionText = true;
						}
						if (music != null) {
							sound.PlayMusic (mapMusic);	
						}
						if (WantTextCloud)
							textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
					} 
				}
			} 
			else {
				if (HelpForTheLargeText >= 0 && HelpForTheLargeText < mission.before_mission_text.Length) {
					if (Input.GetKeyDown (KeyCode.Space)) {
						dBoxx.ShowText (mission.before_mission_text [HelpForTheLargeText]);
						HelpForTheLargeText++;
					}
				} else if (HelpForTheLargeText >= mission.before_mission_text.Length && Input.GetKeyUp (KeyCode.Space)) {
					if (HelpForTheLargeText == mission.before_mission_text.Length)
						HelpForTheLargeText++;
					else if (Input.GetKeyUp (KeyCode.Space)) {
						dBoxx.HideText ();
						pm.dialogueActive = false;
						HelpForTheLargeText = -1;
						if (music != null) {
							sound.PlayMusic (mapMusic);	
						}
						if (WantTextCloud)
							textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
					}

				}
			}
		}





	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			if (Input.GetKeyDown (KeyCode.Space) && HelpForTheLargeText < dialogue.Length) {
				if(mission != null)mission.CheckMissionProgress ();
				if (music != null) {
					sound.PlayMusic (music);	
				}
				Time.timeScale = 0;
				Destroy (textCloud);
				HelpForTheLargeText = 0;
				pm.dialogueActive = true;
				}
			pm.SafeZone = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			Dogo = true;
			if (WantTextCloud)
			textCloud = (GameObject)Instantiate (TextCloud, TextCloudPosition, TextCloud.transform.rotation);
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			pm.SafeZone = false;
			Destroy (textCloud);
			Dogo = false;
		}
	}
}


