using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	Camera cam;
	float leftLim, rightLim;
	GameObject player1;
	public GameObject bg;
	void Start () {
		cam = Camera.main;
		player1 = GameObject.Find ("Player1");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p1pos = player1.transform.position;
		transform.position = new Vector3 (Mathf.Clamp (p1pos.x, leftLim, rightLim), transform.position.y, transform.position.z);
		leftLim = bg.transform.position.x + cam.orthographicSize*cam.aspect;
		rightLim = bg.transform.position.x  + bg.GetComponent<Renderer>().bounds.size.x - cam.orthographicSize*cam.aspect;
	}
}
