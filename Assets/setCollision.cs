using UnityEngine;
using System.Collections;

public class setCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter(Collision col){

		col.gameObject.SendMessage ("addGrounded");

	}

	void OnCollisionExit(Collision col){
		
		col.gameObject.SendMessage ("minusGrounded");
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
