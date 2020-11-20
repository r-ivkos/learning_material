using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour {

	Inventory inventory;
	public bool kill_enemies;
	[Tooltip("THE_LOG, Slime")]
	public string[] enemies_name;
	public int kills;
	public bool bring_object;
	public GameObject obj;
	public bool interact;
	public GameObject thing_to_interact_with;
	public string[] Mission_Completed_Text;
	public string[] before_mission_text;
	public bool giveReward;
	public GameObject Reward;
	bool objectGiven, rewardGiven, missionStartTrigger, thelog, slime;
	Mikel dialogue;
	PickUp objeto;
	Player_Movement pm;

	void Start () {
		dialogue = GetComponent<Mikel> ();
		if(obj!=null)objeto = obj.GetComponent<PickUp> ();
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		pm = GameObject.Find ("Character").GetComponent<Player_Movement> ();
	}
	
	void Update(){
		if (dialogue.missionStarted && !missionStartTrigger) {
			if (kill_enemies && !interact && !bring_object && !dialogue.missionCleared) {
				missionStartTrigger = true;
				pm.missionsActive++;
				foreach (string s in enemies_name) {
					if (s == "THE_LOG") {
						pm.kill_mission_thelog = true;
						pm.missions_thelog++;
						thelog = true;
					}

					if (s == "Slime") { 
						pm.kill_mission_slime = true;
						pm.missions_slime++;
						slime = true;
					}
				}
			}
		}

	}


	public void GiveObject(){
		if (!objectGiven && (!kill_enemies && !interact && bring_object)) {
			inventory.GiveObject (inventory.objectType.IndexOf (objeto.objectID));
			objectGiven = true;
		}
	}
	public void GiveReward(){
		if (giveReward && !rewardGiven) {
			inventory.AddObject (Reward.GetComponent<PickUp> ().objectID);
			rewardGiven = true;
		}
	}
	public void CheckMissionProgress(){
		if (dialogue.missionStarted){
			if (kill_enemies && !interact && !bring_object && !dialogue.missionCleared) {
				
				if (thelog) {
					if (pm.kills_thelog >= kills) {
						dialogue.missionCleared = true;
						pm.missions_thelog--;
						pm.ResetKill (kills);

					}

				} else if (slime) {
					if (pm.kills_slime >= kills) {
						dialogue.missionCleared = true;
						pm.missions_slime--;
						pm.ResetKill (kills);

					}
				} else if (slime && thelog) {
					if ((pm.kills_slime + pm.kills_thelog) >= kills) {
						dialogue.missionCleared = true;
						pm.missions_slime--;
						pm.missions_thelog--;
						pm.ResetKill (kills);

					}
				}


			} else if (!kill_enemies && interact && !bring_object) {


			} else if (!kill_enemies && !interact && bring_object && !dialogue.missionCleared) {
				if (inventory.objectType.Contains (objeto.objectID)) {
					dialogue.missionCleared = true;

				}

			}


		}
	}
}
