using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {


	public Image Health;
	public float hp; 
	public float maxHp = 100f;


	void Start () {
		hp = maxHp;

	}
	
	public void DamageBar (float damage){
		
		hp = Mathf.Clamp (hp - damage, 0, maxHp);
		Health.fillAmount = hp / maxHp;
	}

    public void Heal (float heal){

		hp = Mathf.Clamp (hp + heal, 0, maxHp);
		Health.fillAmount = hp /maxHp;
	}

}
