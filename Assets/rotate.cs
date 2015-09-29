/**
 * Prototype rotation script
 */

using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public GameObject level;
	public GameObject player;
	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			level.transform.Rotate(new Vector3(0,90,0));
			player.transform.Rotate(new Vector3(0,-90,0));
		}
	}
}
