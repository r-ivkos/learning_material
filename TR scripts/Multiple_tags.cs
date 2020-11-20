using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiple_tags : MonoBehaviour {

	// Use this for initialization
	public string[] tags;

	public bool CheckTag(string tag){
			foreach (string s in tags) {
				if (s == tag) {
					return true;
				}
			}
		return false;
	}
}
