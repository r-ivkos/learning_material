using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsManager : MonoBehaviour {
	public Image hp;
	public Image ki;
	void Awake(){
		//hp = transform.GetChild(0).GetComponent<Image>();
		//hp = transform.GetChild(1).GetComponent<Image>();
	}

	public void Manage(float hp, float maxhp, float ki, float maxKi){
		this.hp.fillAmount = Mathf.Clamp01(hp/maxhp);
		this.ki.fillAmount = Mathf.Clamp01(ki/maxKi);
	}
}
