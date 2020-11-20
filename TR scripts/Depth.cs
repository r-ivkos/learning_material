using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour {

	public bool fixEveryFrame;
	SpriteRenderer sprite; 

	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		sprite.sortingLayerName = "Player";
		sprite.sortingOrder = Mathf.RoundToInt (-transform.position.y * 100);
	}
	
	// Update is called once per frame
	void Update () {
		if (fixEveryFrame) {
			sprite.sortingOrder = Mathf.RoundToInt (-transform.position.y * 100);
		}
		
	}
}
