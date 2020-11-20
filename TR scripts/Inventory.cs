using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public Canvas GameUI;
	public Player_Movement pm;
	public Text Name;
	public Text Description;
	public Sprite[] slot_sprite; // 0 - deselected sprite,   1 - selected sprite
	public GameObject descriptionBox;
	public GameObject skillSlot;
	public Sprite[] skill_sprite; // 0 - Slash,  1 - FireBall
	public GameObject Dash;
	public string[] HidenPotential; // 0 - name, 1 - description
	public Sprite[] DashSprite; // 0 - ?,  1 - Dash
	public Image[] MissionObjects;
	public GameObject[] thrownObject;
	string [] DashDescription;
	Image[] eSlots;
	Image[] ssSlots;
	Image[] ssSlotsSkillSelected;
	Image[] moSlots;
	Objects[] obj;
	Objects[] ssObj;
	Image[] ssObjI;
	List<Objects> mObj = new List<Objects> ();
	List<Image> missionObjects = new List<Image>();
	[HideInInspector]
	public List<int> objectType = new List<int> ();
	Canvas C_inv;
	Vector3[] objPos = new Vector3[12]{new Vector3(-354, 78, 0), new Vector3(-254, 78, 0), new Vector3(-152, 78, 0), new Vector3(-56, 78, 0), new Vector3(48, 78, 0), new Vector3(148, 78, 0),
					                   new Vector3(-354, -19, 0), new Vector3(-254, -19, 0), new Vector3(-152, -19, 0), new Vector3(-54, -19, 0), new Vector3(48, -19, 0), new Vector3(148, -19, 0)};
	[HideInInspector]
	public bool invActive;
	[HideInInspector]
	public bool[] slotActive;
	[HideInInspector]
	public int slot_number;

	[HideInInspector]
	public int objects_number;

	void Start () {
		C_inv = GetComponent<Canvas> ();
		DashDescription = new string[2]{ Dash.GetComponent<Objects>().Name, Dash.GetComponent<Objects>().Description};
		C_inv.enabled = false;
		GameUI.enabled = true;
		eSlots = transform.GetChild (3).GetComponentsInChildren<Image>();
		ssSlots = transform.GetChild (1).GetComponentsInChildren<Image> ();
		moSlots = transform.GetChild (5).GetComponentsInChildren<Image> ();
		obj = transform.GetChild (6).GetComponentsInChildren<Objects> ();
		ssObj = transform.GetChild (2).GetComponentsInChildren<Objects> ();
		ssObjI = transform.GetChild (2).GetComponentsInChildren<Image> ();
		ssSlotsSkillSelected = transform.GetChild (4).GetComponentsInChildren<Image> ();
		slot_number = 1;
		objects_number = 0;
		slotActive = new bool[3] { true, false, false }; // 0 - Equipment Slots, 1 - Spells Slots, 2 - Mission Objects Slots
		foreach (Image i in ssSlots) {
			i.enabled = false;
		}
		descriptionBox.SetActive (false);
		int ii = 1;
		foreach (Objects ob in ssObj) {
			ob.empty_slot = !pm.Skills[ii];
			ssObjI[ii-1].enabled = pm.Skills[ii];
			ii = Mathf.Clamp(ii + 1, 0, ssObj.Length - 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		DashUnlock ();
		if (pm.SelectedSkill [0] == false && pm.SelectedSkill [1] == false) {
			skillSlot.SetActive (false);
		} 
		else if (pm.SelectedSkill [0] == true) {
			skillSlot.SetActive (true);
			skillSlot.transform.GetChild (0).GetComponent<Image> ().sprite = skill_sprite [0];
		}
		else if (pm.SelectedSkill [1] == true) {
			skillSlot.SetActive (true);
			skillSlot.transform.GetChild (0).GetComponent<Image> ().sprite = skill_sprite [1];
		}
		if (Input.GetKeyDown (KeyCode.Tab) && !invActive && !pm.dialogueActive && !pm.Slashing && !pm.FireBallCharging) {
			int ii = 1;
			foreach (Objects ob in ssObj) {
				ob.empty_slot = !pm.Skills[ii];
				ssObjI[ii-1].enabled = pm.Skills[ii];
				ii = Mathf.Clamp(ii + 1, 0, ssObj.Length);
			}
			invActive = true;
			GameUI.enabled = false;
			C_inv.enabled = true;
			Time.timeScale = 0f;
			pm.dialogueActive = true;

		} else if (Input.GetKeyDown (KeyCode.Tab) && invActive) {
			invActive = false;
			GameUI.enabled = true;	
			C_inv.enabled = false;
			Time.timeScale = 1f;
			pm.dialogueActive = false;
		}



		if (invActive) {

			if (slotActive [0]) {
				eSlots [slot_number - 1].sprite = slot_sprite [1];
				if (slot_number > 1)
					eSlots [slot_number - 2].sprite = slot_sprite [0];
				if (slot_number < 5)
					eSlots [slot_number].sprite = slot_sprite [0];
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					if (slot_number < 5)
						slot_number = Mathf.Clamp (slot_number + 1, 1, 5);
					else {
						eSlots [slot_number - 1].sprite = slot_sprite [0];
						slot_number = 1;
						slotActive [0] = false;
						slotActive [1] = true;
					}
				} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					slot_number = Mathf.Clamp (slot_number - 1, 1, 5);
				}
				if (Input.GetKeyDown (KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) {
					eSlots [slot_number - 1].sprite = slot_sprite [0];
					slotActive [0] = false;
					slotActive [2] = true;
				}
				InvObjects ();
			} 

			else if (slotActive [1]) {
				foreach (Image i in ssSlots) {
					i.enabled = false;
				}
				ssSlots [slot_number - 1].enabled = true;
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					slot_number = 3;
				}
				else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					if (slot_number == 1) {
						ssSlots [0].enabled = false;
						slot_number = 5;
						slotActive [0] = true;
						slotActive [1] = false;
					} else {
						slot_number = 1;
					}
				}

				else if (Input.GetKeyDown (KeyCode.UpArrow)) {
					slot_number = 2;
				}

				else if (Input.GetKeyDown (KeyCode.DownArrow)) {
					slot_number = 4;
				}
				InvObjects ();
				
			} 

			else if (slotActive [2]) {
				if (slot_number < 7) {
					moSlots [slot_number - 1].sprite = slot_sprite [1];

					if (slot_number > 1)
						moSlots [slot_number - 2].sprite = slot_sprite [0];
					if (slot_number < 6)
						moSlots [slot_number].sprite = slot_sprite [0];
					if (Input.GetKeyDown (KeyCode.RightArrow)) {
						slot_number = Mathf.Clamp (slot_number + 1, 1, 6); 
					} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
						slot_number = Mathf.Clamp (slot_number - 1, 1, 6);
					}
					if (Input.GetKeyUp (KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) {
						moSlots [slot_number - 1].sprite = slot_sprite [0];
						if (slot_number == 6)
							slot_number = 5;
						slotActive [0] = true;
						slotActive [2] = false;
					} 
					else if (Input.GetKeyDown (KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) {
						moSlots [slot_number - 1].sprite = slot_sprite [0];
						slot_number += 6;
					}


				} 
				else {
					moSlots [slot_number - 1].sprite = slot_sprite [1];

					if (slot_number > 7)
						moSlots [slot_number - 2].sprite = slot_sprite [0];
					if (slot_number < 12)
						moSlots [slot_number].sprite = slot_sprite [0];
					
					if (Input.GetKeyDown (KeyCode.RightArrow)) {
						slot_number = Mathf.Clamp (slot_number + 1, 7, 12); 
					} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
						slot_number = Mathf.Clamp (slot_number - 1, 7, 12);
					}
					if (Input.GetKeyUp (KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) {
						moSlots [slot_number - 1].sprite = slot_sprite [0];
						moSlots [slot_number - 1].sprite = slot_sprite [0];
						slot_number -= 6;
						
					} 



				}
				InvObjects ();
			}
				




		}
	}



	void InvObjects(){
		if (slotActive [0]) {
			if (slot_number <= obj.Length) {
				if (!obj [slot_number - 1].empty_slot) {
					descriptionBox.SetActive (true);
					Name.text = obj [slot_number - 1].Name;
					Description.text = obj [slot_number - 1].Description;
				} else
					descriptionBox.SetActive (false);
			} else {
				descriptionBox.SetActive (false);
			} 
		}


		else if (slotActive [1]) {
			if (slot_number <= ssObj.Length) {
				if (!ssObj [slot_number - 1].empty_slot) {
					descriptionBox.SetActive (true);
					Name.text = ssObj [slot_number - 1].Name;
					Description.text = ssObj [slot_number - 1].Description;
					SelectSkill ();
				} else
					descriptionBox.SetActive (false);
			} else
				descriptionBox.SetActive (false);
				
		}


		else if (slotActive [2]) {
			if (slot_number <= mObj.Count) {
				if (!mObj [slot_number - 1].empty_slot) {

					descriptionBox.SetActive (true);
					Name.text = mObj [slot_number - 1].Name;
					Description.text = mObj [slot_number - 1].Description;
					ThrowObject ();
					UseObject ();
				} else {
					descriptionBox.SetActive (false);
				}

			}
			else {
				descriptionBox.SetActive (false);
			}
		}
	
	}

	public void AddObject(int objeto){
		
			objects_number = Mathf.Clamp(objects_number + 1, 0, 13);
			if (objects_number != 13) {
			missionObjects.Add (Instantiate (MissionObjects [objeto], new Vector3(450, 450, 0), Quaternion.identity) as Image);
			missionObjects [objects_number - 1].transform.SetParent(transform.GetChild (7).transform);
				mObj.Add (missionObjects [objects_number - 1].GetComponent<Objects> ());
				missionObjects [objects_number - 1].GetComponent<RectTransform> ().position += objPos [objects_number - 1];
			objectType.Add (objeto);
			}
		}
	void ThrowObject(){
		if (Input.GetKeyDown(KeyCode.Return)){
				missionObjects.Remove (missionObjects [slot_number - 1]); 
				mObj.Remove (mObj [slot_number - 1]);
				if (objects_number == 13)
					objects_number--;
				objects_number--;
				Destroy (transform.GetChild (7).transform.GetChild (slot_number - 1).gameObject);
				int objectnumer = 0;
				foreach (Image i in missionObjects) {
					objectnumer++;
					i.transform.position = new Vector3 (450, 450, 0);
					i.GetComponent<RectTransform> ().position += objPos [objectnumer - 1];
				}
				Instantiate (thrownObject[objectType[slot_number-1]], GameObject.FindGameObjectWithTag ("Player").transform.position + new Vector3 (Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
				objectType.Remove (objectType [slot_number - 1]);	

		}
	}
	public void GiveObject(int index){
		
			missionObjects.Remove (missionObjects [index]); 
			mObj.Remove (mObj [index]);
			if (objects_number == 13)
				objects_number--;
			objects_number--;
			Destroy (transform.GetChild (7).transform.GetChild (index).gameObject);
			int objectnumber = 0;
			foreach (Image i in missionObjects) {
				objectnumber++;
				i.transform.position = new Vector3 (450, 450, 0);
				i.GetComponent<RectTransform> ().position += objPos [objectnumber - 1];
			}
			objectType.Remove (objectType [index]);	

	}

	 void UseObject(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			HealthPotion hpo = mObj [slot_number - 1].GetComponent<HealthPotion> ();
			if (hpo != null) {
				if (hpo.Heal ()) {
					missionObjects.Remove (missionObjects [slot_number - 1]); 
					mObj.Remove (mObj [slot_number - 1]);
					if (objects_number == 13)
						objects_number--;
					objects_number--;
					Destroy (transform.GetChild (7).transform.GetChild (slot_number - 1).gameObject);
					int objectnumber = 0;
					foreach (Image i in missionObjects) {
						objectnumber++;
						i.transform.position = new Vector3 (450, 450, 0);
						i.GetComponent<RectTransform> ().position += objPos [objectnumber - 1];
					}
					objectType.Remove (objectType [slot_number - 1]);
				}
			}
		}
	}


	void SelectSkill(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			foreach (Image im in ssSlotsSkillSelected) {
				im.color = Color.white;
			}
			for (int i = 0; i < pm.SelectedSkill.Length; i++) {
				pm.SelectedSkill [i] = false;
			}
			ssSlotsSkillSelected [slot_number - 1].color = Color.green;
			pm.SelectedSkill [slot_number - 1] = true;
		}
	}

	void DashUnlock(){
		if (!pm.Skills [0]) {
			Dash.GetComponent<Image> ().sprite = DashSprite [0];
			Dash.GetComponent<Objects> ().Name = HidenPotential [0];
			Dash.GetComponent<Objects> ().Description = HidenPotential [1];
		}

		else if (pm.Skills[0]){
			Dash.GetComponent<Image> ().sprite = DashSprite [1];
			Dash.GetComponent<Objects> ().Name = DashDescription [0];
			Dash.GetComponent<Objects> ().Description = DashDescription [1];
		}
	}

}
