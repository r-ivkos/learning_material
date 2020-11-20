using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour {

	Transform player;
	float lx, ly, rx, ry;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
		
	void Update () {
		transform.position = new Vector3 (Mathf.Clamp(player.position.x, lx, rx), Mathf.Clamp(player.position.y, ry, ly), transform.position.z);
	}

	public void BORDES (GameObject map){
		Tiled2Unity.TiledMap tm = map.GetComponent<Tiled2Unity.TiledMap> ();
		float camSize = Camera.main.orthographicSize;

		lx = map.transform.position.x + camSize;
		ly = map.transform.position.y - camSize;
		rx = map.transform.position.x + tm.NumTilesWide - camSize;
		ry = map.transform.position.y - tm.NumTilesHigh + camSize;
	
	}
	void FixedUpdate(){
		Screen.SetResolution (900, 900, false);
	}


	IEnumerator a(){
		yield return new WaitForSeconds (334);

	}
}
	